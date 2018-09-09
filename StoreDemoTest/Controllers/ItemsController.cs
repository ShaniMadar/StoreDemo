using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreDemoTest.Entities;

namespace StoreDemoTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public ItemsController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public IEnumerable<Items> GetItems()
        {
            return _context.Items;
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItems([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = await _context.Items.FindAsync(id);

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT: api/Items
        [HttpPut]
        public async Task<IActionResult> PutItems([FromBody] Items items)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_context.Items.Any(i => i.Id== items.Id))
            {
                return BadRequest("Item id doesn't match any existing item in the database");
            }
            if (!_context.ItemType.Any(e => e.Id == items.ItemType))
            {
                return BadRequest("No Such Item Type");
            }
            if (items.Price < 0)
            {
                return BadRequest("Item price can't be a negative number");
            }
            if (string.IsNullOrEmpty(items.Name))
            {
                return BadRequest("Please provide a valid item name");
            }
            _context.Entry(items).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemsExists(items.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_context.Items.Find(items.Id));
        }

        // POST: api/Items
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] Items item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_context.ItemType.Any(e => e.Id == item.ItemType))
            {
                return BadRequest("No Such Item Type");
            }
            if(item.Price < 0)
            {
                return BadRequest("Item price can't be a negative number");
            }
            if (string.IsNullOrEmpty(item.Name))
            {
                return BadRequest("Please provide a valid item name");
            }
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItems", new { id = item.Id }, item);
        }

        private bool ItemsExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}