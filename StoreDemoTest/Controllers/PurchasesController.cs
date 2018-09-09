using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class PurchasesController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public PurchasesController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public IEnumerable<Purchase> GetPurchase()
        {
            return _context.Purchase;
        }

        // GET: api/Purchases
        [HttpGet("Report/{day}/{month}/{year}")]
        public IEnumerable<Purchase> GetPurchase([FromRoute] int day, [FromRoute] int month, [FromRoute] int year)
        {
            DateTime date;
            try
            {
                date = new DateTime(year, month, day);
            }
            catch(Exception ex)
            {
                //bad date
                return null;
            }
            return _context.Purchase.Where(p => (new DateTime(p.Date.Year, p.Date.Month, p.Date.Day).Equals(date)));
        }

        // GET: api/Purchases
        [HttpGet("Report/{month}/{year}")]
        public IEnumerable<Purchase> GetPurchase([FromRoute] int month, [FromRoute] int year)
        {
            if(month > 12 || month < 1)
            {
                return null;
            }
            if(year < 2000 || year > DateTime.Now.Year)
            {
                return null;
            }
            return _context.Purchase.Where(p => (p.Date.Year == year && p.Date.Month == month));
        }

        // GET: api/Purchases/Employee/1
        [HttpGet("Employee/{id}")]
        public IEnumerable<Purchase> GetReturnsByEmployee([FromRoute] int id)
        {
            return _context.Purchase.Where(p => (p.Employee == id));
        }


        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchase([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var purchase = await _context.Purchase.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(purchase);
        }

        [HttpGet("GetFullData/{id}")]
        public async Task<IActionResult> GetPurchaseFullData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var purchase = Repository.Instance.GetPurchase(id, _context.Database.GetDbConnection().ConnectionString);

            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(purchase);
        }

        [HttpGet("GetDaysReport/{month}/{year}")]
        public string GetDaysReport([FromRoute] int month, [FromRoute] int year)
        {
            DateTime date;
            try
            {
                date = new DateTime(year, month, 1);
            }
            catch(Exception ex)
            {
                return "Please provide a valid month and year";
            }
            return Repository.Instance.GetPurchaseAndReturnReport(date, _context.Database.GetDbConnection().ConnectionString);
        }

        [HttpGet("GetEmployeesReport/{month}/{year}")]
        public string GetEmployeesReport([FromRoute] int month, [FromRoute] int year)
        {
            DateTime date;
            try
            {
                date = new DateTime(year, month, 1);
            }
            catch (Exception ex)
            {
                return "Please provide a valid month and year";
            }
            return Repository.Instance.GetReportPurchaseAndReturnByEmployee(date, _context.Database.GetDbConnection().ConnectionString);
        }

        // POST: api/Purchases
        [HttpPost]
        public async Task<IActionResult> PostPurchase([FromBody] Purchase purchase)
        {
            
            //validate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_context.Employees.Any(e => e.Id == purchase.Employee))
            {
                return BadRequest("Must provide a valid employee Id");
            }
            if (purchase.PurchaseDetails.Count == 0)
            {
                return BadRequest("Purchase must contain at least one Purchase Detail.");
            }
            if (purchase.PurchasePayment.Count == 0)
            {
                return BadRequest("Purchase must contain at least one Purchase Payment.");
            }
            if (!_context.PaymentMethod.Any(pm => pm.Id == purchase.PurchasePayment.FirstOrDefault().PaymentMethod))
            {
                return BadRequest("Payment Method Provided: " + purchase.PurchasePayment.FirstOrDefault().PaymentMethod + " is not valid!");
            }
            
            decimal totalPrice = 0;
            foreach (PurchaseDetails p in purchase.PurchaseDetails)
            {
                if(!_context.Items.Any(i => i.Id == p.Item))
                {
                    return BadRequest("Item id: " + p.Item + " You have provided in the purchase- Doesn't exist");
                }
                if(p.Quantity <= 0)
                {
                    return BadRequest("Item " + p.Item + "'s Quantity is set to 0 or below- This value isn't valid. Please provide a valid quantity value");
                }
                totalPrice += (p.Quantity * (_context.Items.AsNoTracking().SingleOrDefault(i => i.Id == p.Item)).Price);
            }
            purchase.TotalSum = totalPrice;
            if (totalPrice < purchase.PurchasePayment.FirstOrDefault().Sum || purchase.PurchasePayment.FirstOrDefault().Sum==0)
            {
                purchase.PurchasePayment.FirstOrDefault().Sum = totalPrice;
            }
            purchase.Date = DateTime.Now;

            //Try to process the payment
            //if payment is not approved- save the Purchase record under "Pending" status
            //if payment is approved:
            int purchaseId = Repository.Instance.InsertNewPurchase(purchase, _context.Database.GetDbConnection().ConnectionString);
            if(purchaseId > 0)
            {
                foreach(PurchaseDetails p in purchase.PurchaseDetails)
                {
                    p.PurchaseId = purchaseId;
                    p.Id = Repository.Instance.InsertPurchaseDetails(p, _context.Database.GetDbConnection().ConnectionString);
                }
            }

            purchase =  await _context.Purchase.FindAsync(purchaseId);

            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(purchase);
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.Id == id);
        }
    }
}