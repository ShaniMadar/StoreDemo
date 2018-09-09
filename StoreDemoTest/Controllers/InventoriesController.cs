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
    public class InventoriesController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public InventoriesController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/Inventories
        [HttpGet]
        public IEnumerable<Inventory> GetInventory()
        {
            return _context.Inventory;
        }

        // GET: api/Inventories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inventory = await _context.Inventory.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // GET: api/Inventories/ItemId/5
        [HttpGet("ItemId/{id}")]
        public async Task<IActionResult> GetInventoryByItemId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inventory =  _context.Inventory.FirstOrDefault(i => i.Item == id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // PUT: api/Inventories
        [HttpPut]
        public async Task<IActionResult> PutInventory([FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //validate
            if(inventory.Quantity < 0)
            {
                return BadRequest("Can't assign value lower than 0 to inventory quantity");
            }
            if(!_context.Employees.Any(e =>e.Id == inventory.Employee))
            {
                return BadRequest("Must provide a valid Employee Id in order to update inventory");
            }
            if(!_context.Inventory.Any(i => i.Item == inventory.Item))
            {
                return BadRequest("This item doesn't exist in the inventory, you could add it.");
            }
            
            Inventory inventoryUpdate = _context.Inventory.AsNoTracking().Single(i => i.Item == inventory.Item);
            
            //assign the object for updating
            inventory.LastUpdated = DateTime.Now;
            inventory.Id = inventoryUpdate.Id;

            //update
            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(inventoryUpdate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(inventory);
        }

        // POST: api/Inventories
        [HttpPost]
        public async Task<IActionResult> PostInventory([FromBody] Inventory inventory)
        {

            //validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (inventory.Quantity < 0)
            {
                return BadRequest("Can't assign value lower than 0 to inventory quantity");
            }
            if (_context.Inventory.Any(i => i.Item == inventory.Item))
            {
                return BadRequest("This item already exist in the inventory, you could update it's quantity.");
            }
            if (!_context.Employees.Any(e => e.Id == inventory.Employee))
            {
                return BadRequest("Must provide a valid Employee Id in order to update inventory");
            }

            inventory.LastUpdated = DateTime.Now;

            _context.Inventory.Add(inventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventory", new { id = inventory.Id }, inventory);
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.Id == id);
        }
    }
}