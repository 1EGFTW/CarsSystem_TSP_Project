#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarSystem_TSP_Project.Models;
using CarsSystem_TSP_Project.Data;
using Microsoft.AspNetCore.Authorization;

namespace CarsSystem_TSP_Project.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Service> GetSearchResults(String search)
        {
            var searchByServiceType = _context.Services.Include(c => c.Mechanics).Where(t => t.Type.Equals(search)).ToList();
            if (searchByServiceType.Count > 0)
                return searchByServiceType;
            var searchCriteria = 0.0;
            try
            {
                searchCriteria = double.Parse(search);
            }
            catch (Exception ex) { }

            var searchByServicePrice = _context.Services.Include(c => c.Mechanics).Where(t => t.Price>=searchCriteria).ToList();
            if (searchByServicePrice.Count > 0)
                return searchByServicePrice;

            var searchServiceByMechanicName = _context.Services.Include(c => c.Mechanics).Where(t => t.Mechanics.Name.Equals(search)).ToList();
            if (searchServiceByMechanicName.Count > 0)
                return searchServiceByMechanicName;

            return null;
        }
        // GET: Services
        public async Task<IActionResult> Index(String search) //search by type/price
        {
            var applicationDbContext = _context.Services.Include(c => c.Mechanics);
            if (String.IsNullOrEmpty(search)) //if nothing is typed in the search bar
            {
                return View(await applicationDbContext.ToListAsync()); //returns all entries in DB
            }
            if (GetSearchResults(search) != null)
            {
                return View(GetSearchResults(search));
            }
            return View(await applicationDbContext.ToListAsync());
        }

        /*// GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
*/
        // GET: Services/AddOrEdit
        [Authorize]
        public IActionResult AddOrEdit(int id)
        {
            if (id == 0)
            {
                ViewData["MechanicId"] = new SelectList(_context.Mechanics, "MechanicId", "Name");
                return View(new Service());
            }


            else
            {
                ViewData["MechanicId"] = new SelectList(_context.Mechanics, "MechanicId", "Name");
                return View(_context.Services.Find(id));
            }
        }

        // POST: Services/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("ServiceId,Name,Type,Price,MechanicId")] Service service)
        {/*
            if (ModelState.IsValid)
            {*/
            if (service.ServiceId == 0)
                _context.Add(service);
            else
                _context.Update(service);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            /* }
             return View(service);*/
        }
        /*
                // GET: Services/Edit/5
                public async Task<IActionResult> Edit(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var service = await _context.Services.FindAsync(id);
                    if (service == null)
                    {
                        return NotFound();
                    }
                    return View(service);
                }

                // POST: Services/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("ServiceId,Name,Type,Price")] Service service)
                {
                    if (id != service.ServiceId)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(service);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ServiceExists(service.ServiceId))
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
                    return View(service);
                }
        */
        // GET: Services/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}