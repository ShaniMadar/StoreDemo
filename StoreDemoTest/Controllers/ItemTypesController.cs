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
    public class ItemTypesController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public ItemTypesController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/ItemTypes
        [HttpGet]
        public IEnumerable<ItemType> GetItemType()
        {
            return _context.ItemType;
        }

        // GET: api/ItemTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemType = await _context.ItemType.FindAsync(id);

            if (itemType == null)
            {
                return NotFound();
            }

            return Ok(itemType);
        }

        // PUT: api/ItemTypes
        [HttpPut]
        public async Task<IActionResult> PutItemType([FromBody] ItemType itemType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (itemType.Id <= 0)
            {
                return BadRequest("please provide a valid id");
            }
            if (_context.ItemType.Any(e => e.Name.Equals(itemType.Name) && e.Id != itemType.Id))
            {
                return BadRequest("This Item Type: "+itemType.Name+" already exists");
            }
            if (string.IsNullOrEmpty(itemType.Name))
            {
                return BadRequest("Please provide a valid item name");
            }

            if (itemType.ReturnPeriod < 0)
                itemType.ReturnPeriod = 0;

            _context.Entry(itemType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemTypeExists(itemType.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_context.ItemType.Find(itemType.Id));
        }

        // POST: api/ItemTypes
        [HttpPost]
        public async Task<IActionResult> AddItemType([FromBody] ItemType itemType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.ItemType.Any(e => e.Name.Equals(itemType.Name)))
            {
                return BadRequest("This Item Type " + itemType.Name + " already exists");
            }
            if (string.IsNullOrEmpty(itemType.Name))
            {
                return BadRequest("Please provide a valid item name");
            }

            if (itemType.ReturnPeriod < 0)
                itemType.ReturnPeriod = 0;

            _context.ItemType.Add(itemType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemType", new { id = itemType.Id }, itemType);
        }

        private bool ItemTypeExists(int id)
        {
            return _context.ItemType.Any(e => e.Id == id);
        }
    }
}