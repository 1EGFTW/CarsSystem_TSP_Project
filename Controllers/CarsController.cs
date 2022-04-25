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
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(String search)
        {
                var applicationDbContext = _context.Cars.Include(c => c.Owner).Include(c => c.Payment).Include(c => c.Services);

            if (String.IsNullOrEmpty(search)) //if nothing is typed in the search bar
            {
                return View(await applicationDbContext.ToListAsync()); //returns all entries in DB
            }
            
            else
            {
                var searchByOwnerName = await _context.Cars.Include(c => c.Owner).Include(c => c.Payment).Include(c => c.Services)
                    .Where(t => t.Owner.Name.Equals(search)).ToListAsync(); //checks if search param is for owner

                var searcrByCarManufacturer = await _context.Cars.Include(c => c.Owner).Include(c => c.Payment).Include(c => c.Services)
                     .Where(t => t.Manufacturer.Equals(search)).ToListAsync(); //checks if search param is for manufacturer
                var priceToSearch=0.0; //var for price checking
                try
                {
                    priceToSearch = Double.Parse(search); //exception handling for parsing
                }
                catch (ArgumentNullException ane) { }
                catch (FormatException fe) { }
                catch (OverflowException oe) { }
                
                var searcrByPrice = await _context.Cars.Include(c => c.Owner).Include(c => c.Payment).Include(c => c.Services)
                    .Where(t => t.Price>=priceToSearch).ToListAsync(); //check if search param is for price
                if (searchByOwnerName.Count > 0) //if there are records found by owner name
                {
                    return View(searchByOwnerName);
                }
                else if (searcrByCarManufacturer.Count>0) //if there are records found by manufacturer
                {
                    return View(searcrByCarManufacturer);
                }
                else if (searcrByPrice.Count > 0) //if there are records found by price
                {
                    return View(searcrByPrice);
                }
                else
                    return View(await applicationDbContext.ToListAsync()); //default


            }
            
           
        }

        /*  // GET: Cars/Details/5
          public async Task<IActionResult> Details(int? id)
          {
              if (id == null)
              {
                  return NotFound();
              }

              var car = await _context.Cars
                  .Include(c => c.Owner)
                  .Include(c => c.Payment)
                  .Include(c => c.Services)
                  .FirstOrDefaultAsync(m => m.CarId == id);
              if (car == null)
              {
                  return NotFound();
              }

              return View(car);
          }*/

        // GET: Cars/AddOrEdit
        [Authorize]
        public IActionResult AddOrEdit(int id)
        {
            if (id == 0)
            {
                ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "Name");
                ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "Type");
                ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name");
                return View(new Car());
            }
            else
            {
                ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "Name");
                ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "Type");
                ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name");
                return View(_context.Cars.Find(id));
            }
        }

        // POST: Cars/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("CarId,Manufacturer,Model,Engine,Transmission,DriveType,Vin,Price,DateOfFirstReg,Mileage,OwnerId,PaymentId,Discount,VehicleType,ServiceId")] Car car)
        {
            // if (ModelState.IsValid)
            //  {
            Owner owner = _context.Owners.Find(car.OwnerId);
              if(car.CarId == 0)
            {
                if(!owner.Name.Equals("No owner"))
                {
                owner.CarsBought++;
                _context.Update(owner);
                }
                car.Price = car.Price - (car.Price * car.Discount)/100;
                _context.Add(car);

            }
            else
            {
                if (!owner.Name.Equals("No owner"))
                {
                    owner.CarsBought++;
                    _context.Update(owner);
                }
                car.Price = car.Price - (car.Price * car.Discount)/100;
                _context.Add(car);
                _context.Update(car);
            }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
          //  }
                
            
         /*   ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "Name", car.OwnerId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "Type", car.PaymentId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name", car.ServiceId);
            return View(car);*/
        }

        /*   // GET: Cars/Edit/5
           public async Task<IActionResult> Edit(int? id)
           {
               if (id == null)
               {
                   return NotFound();
               }

               var car = await _context.Cars.FindAsync(id);
               if (car == null)
               {
                   return NotFound();
               }
               ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "Name", car.OwnerId);
               ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "Type", car.PaymentId);
               ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name", car.ServiceId);
               return View(car);
           }

           // POST: Cars/Edit/5
           // To protect from overposting attacks, enable the specific properties you want to bind to.
           // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
           [HttpPost]
           [ValidateAntiForgeryToken]
           public async Task<IActionResult> Edit(int id, [Bind("CarId,Manufacturer,Model,Engine,Transmission,DriveType,Vin,Price,DateOfFirstReg,Mileage,OwnerId,PaymentId,Discount,VehicleType,ServiceId")] Car car)
           {
               if (id != car.CarId)
               {
                   return NotFound();
               }

               if (ModelState.IsValid)
               {
                   try
                   {
                       _context.Update(car);
                       await _context.SaveChangesAsync();
                   }
                   catch (DbUpdateConcurrencyException)
                   {
                       if (!CarExists(car.CarId))
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
               ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "Name", car.OwnerId);
               ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "Type", car.PaymentId);
               ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name", car.ServiceId);
               return View(car);
           }
   */
        // GET: Cars/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Owner)
                .Include(c => c.Payment)
                .Include(c => c.Services)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
