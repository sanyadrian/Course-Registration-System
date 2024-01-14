using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab6.DataAccess;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Lab6.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly StudentrecordContext _context;

        public EmployeesController(StudentrecordContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
              return _context.Employees != null ? 
                          View(await _context.Employees.Include(a=>a.Roles).ToListAsync()) :
                          Problem("Entity set 'StudentrecordContext.Employees'  is null.");
        }

        // GET: Employees/Details/5

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AvailableRoles"] = await _context.Roles.ToListAsync();
            return View(new Employee());
        }



        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, List<int> SelectedRoles)
        {
            if(SelectedRoles == null || !SelectedRoles.Any()){
                ModelState.AddModelError("Roles", "You must select at least on role!");
            }
            if(_context.Employees.Any(e=>e.UserName == employee.UserName && e.Id != employee.Id)) {
                ModelState.AddModelError("UserName", "This user name has been used by another employee!");
            }
            if (ModelState.IsValid)
            {

                employee.Roles.Clear();

                foreach (var roleId in SelectedRoles)
                {
                    var role = await _context.Roles.FindAsync(roleId);
                    if (role != null)
                    {
                        employee.Roles.Add(role);
                    }
                }

                _context.Add(employee);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

    ViewData["AvailableRoles"] = await _context.Roles.ToListAsync();
    return View(employee);
}


        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                     .Include(e => e.Roles)
                     .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            ViewData["AvailableRoles"] = await _context.Roles.ToListAsync();
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee, List<int> SelectedRoles)
        {
            if(SelectedRoles == null || !SelectedRoles.Any()){
                ModelState.AddModelError("Roles", "You must select at least on role!");
                ViewData["AvailableRoles"] = await _context.Roles.ToListAsync();
                return View(employee);
            }

            if (!ModelState.IsValid)
            {
                ViewData["AvailableRoles"] = await _context.Roles.ToListAsync();
                return View(employee);
            }

            
            if(_context.Employees.Any(e=>e.UserName == employee.UserName && e.Id != employee.Id)) {
                ModelState.AddModelError("UserName", "This user name has been used by another employee!");
            }
            if (ModelState.IsValid)
            {
                if (!EmployeeExists(employee.Id))
                {
                    return NotFound();
                }

                try
                {
                    var existingEmployee = await _context.Employees
                                                .Include(e => e.Roles)
                                                .FirstOrDefaultAsync(e => e.Id == employee.Id);

                    if (existingEmployee != null)
                    {
                        existingEmployee.Roles.Clear();

                        foreach (var roleId in SelectedRoles.Distinct())
                        {
                            var role = await _context.Roles.FindAsync(roleId);
                            if (role != null)
                            {
                                existingEmployee.Roles.Add(role);
                            }
                        }

                        _context.Update(existingEmployee);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
