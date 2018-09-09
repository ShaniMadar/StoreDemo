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
    public class EmployeesController : ControllerBase
    {
        private readonly StoreDemoTestContext _context;

        public EmployeesController(StoreDemoTestContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public IEnumerable<Employees> GetEmployees()
        {
            return _context.Employees;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employees = await _context.Employees.FindAsync(id);

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        // PUT: api/Employees
        [HttpPut]
        public async Task<IActionResult> PutEmployees([FromBody] Employees employees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_context.Employees.Any(e1 => e1.Id == employees.Id))
            {
                return BadRequest("Employee doesn't exist");
            }
            if (string.IsNullOrEmpty(employees.FirstName) || string.IsNullOrEmpty(employees.LastName))
            {
                return BadRequest("must provide valid first name and last name for the employee");
            }

            Employees e = _context.Employees.AsNoTracking().SingleOrDefault(ee => ee.Id == employees.Id);

            employees.StartDate = e.StartDate;

            _context.Entry(employees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(employees.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(_context.Employees.Find(employees.Id));
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployees([FromBody] Employees employees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(employees.FirstName) || string.IsNullOrEmpty(employees.LastName))
            {
                return BadRequest("must provide valid first name and last name for the employee");
            }
            employees.StartDate = DateTime.Now;

            _context.Employees.Add(employees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployees", new { id = employees.Id }, employees);
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}