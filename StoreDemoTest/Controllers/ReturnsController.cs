using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreDemoTest.Entities;
using StoreDemoTest.Helpers;

namespace StoreDemoTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnsController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public ReturnsController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/Returns
        [HttpGet]
        public IEnumerable<Returns> GetReturns()
        {
            return _context.Returns;
        }

        // GET: api/Returns/PurchaseID/1022
        [HttpGet("PurchaseID/{id}")]
        public IEnumerable<Returns> GetReturnsByPurchaseId([FromRoute] int id)
        {
            return _context.Returns.Where(p => p.PurchaseDetail.Purchase.Id == id);
        }

        // GET: api/Returns/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReturns([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var returns = await _context.Returns.FindAsync(id);

            if (returns == null)
            {
                return NotFound();
            }

            return Ok(returns);
        }

        // GET: api/Returns
        [HttpGet("Report/{day}/{month}/{year}")]
        public IEnumerable<Returns> GetReturns([FromRoute] int day, [FromRoute] int month, [FromRoute] int year)
        {
            DateTime date;
            try
            {
                date = new DateTime(year, month, day);
            }
            catch (Exception ex)
            {
                //bad date
                return null;
            }
            return _context.Returns.Where(p => (new DateTime(p.Date.Year, p.Date.Month, p.Date.Day).Equals(date)));
        }

        // GET: api/Returns
        [HttpGet("Report/{month}/{year}")]
        public IEnumerable<Returns> GetReturns([FromRoute] int month, [FromRoute] int year)
        {
            if (month > 12 || month < 1)
            {
                return null;
            }
            if (year < 2000 || year > DateTime.Now.Year)
            {
                return null;
            }
            return _context.Returns.Where(p => (p.Date.Year == year && p.Date.Month == month));
        }

        // GET: api/Returns/Employee/1
        [HttpGet("Employee/{id}")]
        public IEnumerable<Returns> GetReturnsByEmployee([FromRoute] int id)
        {
            return _context.Returns.Where(p => (p.Employee==id));
        }

        // POST: api/Returns
        [HttpPost]
        public async Task<IActionResult> PostReturns([FromBody] Purchase purchase)
        {
            //Validate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Validate the purchase and its status
            if(!_context.Purchase.Any(pp => pp.Id == purchase.Id))
            {
                return BadRequest("Purchase doesn't exist");
            }
            if(purchase.PurchaseDetails.ToList().FirstOrDefault().Quantity <= 0)
            {
                return BadRequest("Can't return less than 1 item. Please correct the Quantity value");
            }
            if (!_context.Employees.Any(e => e.Id == purchase.Employee))
            {
                return BadRequest("Employee id: "+purchase.Employee+" doesn't exist. Please provide a valid employee id");
            }
            if (!_context.Items.Any(e => e.Id == purchase.PurchaseDetails.ToList().FirstOrDefault().Item))
            {
                return BadRequest("Item id: " + purchase.PurchaseDetails.ToList().FirstOrDefault().Item + " doesn't exist. Please provide a valid Item id");
            }
            Purchase p = _context.Purchase.AsNoTracking().FirstOrDefault(p2 => p2.Id == purchase.Id);

            if(p.Status!= _context.PurchaseStatusType.AsNoTracking().FirstOrDefault(pst => pst.Name == "Completed").Id 
                && p.Status != _context.PurchaseStatusType.AsNoTracking().FirstOrDefault(pst => pst.Name == "Partially Credited").Id)
            {
                return BadRequest("Purchase status is:" + _context.PurchaseStatusType.AsNoTracking().FirstOrDefault(pst => pst.Id == p.Status).Name + ". Only purchases which are completed or partially returned can be returned.");
            }

            //payment method for modification if will be required
            if (!_context.PaymentMethod.Any(pm => pm.Id == purchase.PurchasePayment.ToList().FirstOrDefault().PaymentMethod))
            {
                return BadRequest("please provide a valid credit method. The method id you provided: " + purchase.PurchasePayment.ToList().FirstOrDefault().PaymentMethod + " doesn't exist");
            }
            PaymentMethod paymentMethod = _context.PaymentMethod.AsNoTracking().FirstOrDefault(pm => pm.Id == purchase.PurchasePayment.ToList().FirstOrDefault().PaymentMethod);

            //preparing the detail object to check its validity to be returned.
            PurchaseDetails purchaseDetail = _context.PurchaseDetails.AsNoTracking().FirstOrDefault(pd => (pd.Item == (purchase.PurchaseDetails.ToList().FirstOrDefault().Item) && pd.PurchaseId == purchase.Id));
            if(purchaseDetail == null)
            {
                return BadRequest("This item wasn't purchased in the requested purchase id");
            }
            purchaseDetail.Quantity = purchase.PurchaseDetails.ToList().FirstOrDefault().Quantity;

            //purchaseDetail.PurchaseId = purchase.Id;

            //validate return option by the purchase detail
            var valid = validateReturn(purchaseDetail);
            switch (valid)
            {
                case ReturnMethod.NoReturnType:
                    return BadRequest("This Item Can't be returned due to its item type");
                case ReturnMethod.NoReturnDate:
                    return BadRequest("This item can't be returned, it has been over 30 days since it has been purchased");
                case ReturnMethod.NoReturnQuantity:
                    return BadRequest("This return can't be completed, the quantity you requested to return is higher than the purchased quantity.");
                case ReturnMethod.NoReturnPaidAmount:
                    return BadRequest("This item can't be returned, the purchase paid amount is less than the value of the return item");
                case ReturnMethod.VoucherOnly:
                    paymentMethod = _context.PaymentMethod.Find(3); //voucher
                    break;
                case ReturnMethod.AnyReturn:
                    //improvment suggesstion: 
                    //validate the amount that was paid via this payment method hasn't exceeded the returned amount via this payment method.
                    paymentMethod = _context.PaymentMethod.Find(paymentMethod.Id);
                    break;

            }

            //prepare the return
            Returns returns = new Returns();
            returns.PurchaseDetailId = purchaseDetail.Id;
            returns.Quantity = purchaseDetail.Quantity;
            returns.CreditType = paymentMethod.Id;

            //Insert Return
            int returnId = Repository.Instance.InsertNewReturn(returns, _context.Database.GetDbConnection().ConnectionString);
            
            //Return the updated Purchase object.
            Purchase r = Repository.Instance.GetPurchase(purchase.Id, _context.Database.GetDbConnection().ConnectionString);
            if (r == null)
            {
                return NotFound();
            }
            return Ok(r);
        }

        private ReturnMethod validateReturn(PurchaseDetails purchaseDetail)
        {

            //Stored Procedure to validate
            ReturnMethod returnMethod = Repository.Instance.ValidateReturn(purchaseDetail, _context.Database.GetDbConnection().ConnectionString);

            return returnMethod;
        }

        private bool ReturnsExists(int id)
        {
            return _context.Returns.Any(e => e.Id == id);
        }
    }
}