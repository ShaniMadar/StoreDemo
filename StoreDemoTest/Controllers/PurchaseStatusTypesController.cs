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
    public class PurchaseStatusTypesController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public PurchaseStatusTypesController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseStatusTypes
        [HttpGet]
        public IEnumerable<PurchaseStatusType> GetPurchaseStatusType()
        {
            return _context.PurchaseStatusType;
        }

        // GET: api/PurchaseStatusTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseStatusType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var purchaseStatusType = await _context.PurchaseStatusType.FindAsync(id);

            if (purchaseStatusType == null)
            {
                return NotFound();
            }

            return Ok(purchaseStatusType);
        }

        // PUT: api/PurchaseStatusTypes
        [HttpPut]
        public async Task<IActionResult> PutPurchaseStatusType([FromBody] PurchaseStatusType purchaseStatusType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(purchaseStatusType.Id <= 0)
            {
                return BadRequest("please provide a valid id");
            }
            if (_context.PurchaseStatusType.Any(pst => pst.Name.Equals(purchaseStatusType.Name) && pst.Id != purchaseStatusType.Id))
            {
                return BadRequest("This purchase status type: " + purchaseStatusType.Name + " already exists in the database");
            }

            _context.Entry(purchaseStatusType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseStatusTypeExists(purchaseStatusType.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_context.PurchaseStatusType.Find(purchaseStatusType.Id));
        }

        // POST: api/PurchaseStatusTypes
        [HttpPost]
        public async Task<IActionResult> PostPurchaseStatusType([FromBody] PurchaseStatusType purchaseStatusType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(_context.PurchaseStatusType.Any(pst => pst.Name.Equals(purchaseStatusType.Name)))
            {
                return BadRequest("This purchase status type: " + purchaseStatusType.Name + " already exists in the database");
            }

            _context.PurchaseStatusType.Add(purchaseStatusType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseStatusType", new { id = purchaseStatusType.Id }, purchaseStatusType);
        }

        private bool PurchaseStatusTypeExists(int id)
        {
            return _context.PurchaseStatusType.Any(e => e.Id == id);
        }
    }
}