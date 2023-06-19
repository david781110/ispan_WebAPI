using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeServices.Models;
using System.Reflection;
using EmployeeServices.DTO;

namespace EmployeeServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<EmployeeDTO>> GetEmployees()
        {

            return _context.Employees.Select(emp => new EmployeeDTO
            {
                EmployeeID = emp.EmployeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Title = emp.Title
            });
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployees(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employees = await _context.Employees.FindAsync(id);



            if (employees == null)
            {
                return Content("沒有這筆資料");
            }

            return new EmployeeDTO
            {
                EmployeeID = employees.EmployeeId,
                FirstName = employees.FirstName,
                LastName = employees.LastName,
                Title = employees.Title
            };
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployees(int id, EmployeeDTO employees)
        {
            if (id != employees.EmployeeID)
            {
                return BadRequest();
            }
            Employees emp = await _context.Employees.FindAsync(id);
            emp.FirstName = employees.FirstName;
            emp.LastName = employees.LastName;
            emp.Title = employees.Title;
            _context.Entry(emp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content("修改成功!");
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employees>> PostEmployees(EmployeeDTO employees)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'NorthwindContext.Employees'  is null.");
            }
            Employees emp = new Employees
            {
                FirstName = employees.FirstName,
                LastName = employees.LastName,
                Title = employees.Title
            };
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            return Content("新增紀錄成功!");
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployees(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            try
            {
                _context.Employees.Remove(employees);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Content("刪除失敗");

            }


            return Content("刪除成功!");
        }

        private bool EmployeesExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
