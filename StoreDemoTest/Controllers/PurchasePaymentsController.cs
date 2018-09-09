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
    public class PurchasePaymentsController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public PurchasePaymentsController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/PurchasePayments
        [HttpGet]
        public IEnumerable<PurchasePayment> GetPurchasePayment()
        {
            return _context.PurchasePayment;
        }

        // GET: api/PurchasePayments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchasePayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var purchasePayment = await _context.PurchasePayment.FindAsync(id);

            if (purchasePayment == null)
            {
                return NotFound();
            }

            return Ok(purchasePayment);
        }

        // POST: api/PurchasePayments
        [HttpPost]
        public async Task<IActionResult> PostPurchasePayment([FromBody] PurchasePayment purchasePayment)
        {
            //validate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_context.Purchase.Any(p => p.Id == purchasePayment.PurchaseId))
            {
                return BadRequest("This Purchase doesn't exist");
            }
            if (!_context.PaymentMethod.Any(p => p.Id == purchasePayment.PaymentMethod))
            {
                return BadRequest("This Payment Method doesn't exist");
            }
            Purchase purchase = _context.Purchase.AsNoTracking().SingleOrDefault(p => p.Id == purchasePayment.PurchaseId);
            if(purchase.TotalSum - purchase.PaidAmount <= 0)
            {
                return BadRequest("Already completed all payments for this purchase");
            }
            if (purchase.TotalSum - purchase.PaidAmount < purchasePayment.Sum || purchasePayment.Sum <= 0)
            {
                purchasePayment.Sum = purchase.TotalSum - (decimal)purchase.PaidAmount;
            }

            // complete payment and receive the response
            //if(!paymentApproved)
            //return - payment declined.

            //Insert Payment to DB
            int purchaseStatus = Repository.Instance.InsertPurchasePayment(purchasePayment, _context.Database.GetDbConnection().ConnectionString);
            
            //Retreive the Updated Purchase
            Purchase updatedPurchase = Repository.Instance.GetPurchase(purchasePayment.PurchaseId, _context.Database.GetDbConnection().ConnectionString);

            if (updatedPurchase == null)
            {
                return NotFound();
            }
            return Ok(updatedPurchase);
        }

        private bool PurchasePaymentExists(int id)
        {
            return _context.PurchasePayment.Any(e => e.Id == id);
        }
    }
}