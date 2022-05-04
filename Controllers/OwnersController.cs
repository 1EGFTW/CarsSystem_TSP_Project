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
    public class OwnersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OwnersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Owner> GetSearchResults(String search)
        {
            var searchCriteria = 0;
            try
            {
                searchCriteria = int.Parse(search);
            }
            catch (Exception ex) { }

            var searchByCarsBought = _context.Owners.Where(t => t.CarsBought >= searchCriteria).ToList();
            if (searchByCarsBought.Count > 0)
                return searchByCarsBought;

            return null;
            
        }
        // GET: Owners
        public async Task<IActionResult> Index(String search) //search za cars bought
        {
            if (String.IsNullOrEmpty(search)) //if nothing is typed in the search bar
            {
                return View(await _context.Owners.ToListAsync()); //returns all entries in DB
            }
            else if (GetSearchResults(search) != null)
            {
                return View(GetSearchResults(search));
            }
            return View(await _context.Owners.ToListAsync());
        }

        /*// GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .FirstOrDefaultAsync(m => m.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }*/

        // GET: Owners/AddOrEdit
        [Authorize]
        public IActionResult AddOrEdit(int id)
        {
            if (id == 0)
                return View(new Owner());
            else return View(_context.Owners.Find(id));
        }

        // POST: Owners/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("OwnerId,Name,CarsBought")] Owner owner)
        {
            /*  if (ModelState.IsValid)
              {*/
            if (owner.OwnerId == 0)
                _context.Add(owner);
            else
                _context.Update(owner);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
           /* }
            return View(owner);*/
        }

        /* // GET: Owners/Edit/5
         public async Task<IActionResult> Edit(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var owner = await _context.Owners.FindAsync(id);
             if (owner == null)
             {
                 return NotFound();
             }
             return View(owner);
         }*/

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, [Bind("OwnerId,Name,CarsBought")] Owner owner)
         {
             if (id != owner.OwnerId)
             {
                 return NotFound();
             }

             if (ModelState.IsValid)
             {
                 try
                 {
                     _context.Update(owner);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!OwnerExists(owner.OwnerId))
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
             return View(owner);
         }*/

        // GET: Owners/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .FirstOrDefaultAsync(m => m.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.OwnerId == id);
        }
    }
}
