using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreDemoTest.Entities;

namespace StoreDemoTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public PaymentMethodsController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/PaymentMethods
        [HttpGet]
        public IEnumerable<PaymentMethod> GetPaymentMethod()
        {
            return _context.PaymentMethod;
        }

        // GET: api/PaymentMethods/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethod([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentMethod = await _context.PaymentMethod.FindAsync(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return Ok(paymentMethod);
        }

        // PUT: api/PaymentMethods
        [HttpPut]
        public async Task<IActionResult> PutPaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_context.PaymentMethod.Any(pm => pm.Id == paymentMethod.Id))
            {
                return BadRequest("Payment method id: "+paymentMethod.Id+" doesn't exist");
            }
            if (string.IsNullOrEmpty(paymentMethod.Name))
            {
                return BadRequest("Payment method name can't be empty");
            }
            if (_context.PaymentMethod.Any(pm => pm.Name.Equals(paymentMethod.Name)&& pm.Id != paymentMethod.Id))
            {
                return BadRequest("Payment method: " + paymentMethod.Name + " already exists");
            }

            _context.Entry(paymentMethod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentMethodExists(paymentMethod.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_context.PaymentMethod.Find(paymentMethod.Id));
        }

        // POST: api/PaymentMethods
        [HttpPost]
        public async Task<IActionResult> PostPaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(paymentMethod.Name))
            {
                return BadRequest("Payment method name can't be empty");
            }
            if (_context.PaymentMethod.Any(pm => pm.Name.Equals(paymentMethod.Name)))
            {
                return BadRequest("Payment method: "+paymentMethod.Name+" already exists");
            }
            _context.PaymentMethod.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentMethod", new { id = paymentMethod.Id }, paymentMethod);
        }

        // DELETE: api/PaymentMethods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentMethod = await _context.PaymentMethod.FindAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            _context.PaymentMethod.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(paymentMethod);
        }

        private bool PaymentMethodExists(int id)
        {
            return _context.PaymentMethod.Any(e => e.Id == id);
        }
    }
}