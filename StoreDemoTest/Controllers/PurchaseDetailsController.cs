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
    public class PurchaseDetailsController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public PurchaseDetailsController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseDetails
        [HttpGet]
        public IEnumerable<PurchaseDetails> GetPurchaseDetails()
        {
            return _context.PurchaseDetails;
        }

        // GET: api/PurchaseDetails/PurchaseID/1022
        [HttpGet("PurchaseID/{id}")]
        public IEnumerable<PurchaseDetails> GetReturnsByPurchaseId([FromRoute] int id)
        {
            return _context.PurchaseDetails.Where(p => p.Purchase.Id == id);
        }

        // GET: api/PurchaseDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var purchaseDetails = await _context.PurchaseDetails.FindAsync(id);

            if (purchaseDetails == null)
            {
                return NotFound();
            }

            return Ok(purchaseDetails);
        }

        private bool PurchaseDetailsExists(int id)
        {
            return _context.PurchaseDetails.Any(e => e.Id == id);
        }
    }
}