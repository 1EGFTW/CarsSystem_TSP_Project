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

        public IEnumerable<Car> GetSearchResults(String search)
        {
            var searchByOwnerName =  _context.Cars.Include(c => c.Owner).Include(c => c.Payment).Include(c => c.Services)
                  .Where(t => t.Owner.Name.Equals(search)).ToList(); //checks if search param is for owner
            if (searchByOwnerName.Count > 0)
            {
                return searchByOwnerName;
            }
            var searcrByCarManufacturer = _context.Cars.Include(c => c.Owner).Include(c => c.Payment).Include(c => c.Services)
                    .Where(t => t.Manufacturer.Equals(search)).ToList();//checks if search param is for manufacturer
            if (searcrByCarManufacturer.Count > 0)
            {
                return searcrByCarManufacturer;
            }
            var priceToSearch = 0.0; //var for price checking
            try
            {
                priceToSearch = Double.Parse(search); //exception handling for parsing
            }
            catch (ArgumentNullException ane) { }
            catch (FormatException fe) { }
            catch (OverflowException oe) { }

            var searcrByPrice =  _context.Cars.Include(c => c.Owner).Include(c => c.Payment).Include(c => c.Services)
                .Where(t => t.Price >= priceToSearch).ToList();//check if search param is for price
            if (searcrByPrice.Count > 0)
            {
                return searcrByPrice;
            }
            return null;

        }

        // GET: Cars
        public async Task<IActionResult> Index(String search) 
        {
            var applicationDbContext = _context.Cars.Include(c => c.Owner).Include(c => c.Payment).Include(c => c.Services);

            if (String.IsNullOrEmpty(search)) //if nothing is typed in the search bar
            {
                return View(await applicationDbContext.ToListAsync()); //returns all entries in DB
            }
            
            else if(GetSearchResults(search)!=null)
            {
                return View(GetSearchResults(search));

            }
            else return View(await applicationDbContext.ToListAsync());//default


        }

 

        // GET: Cars/AddOrEdit
        [Authorize]
        public IActionResult AddOrEdit(int id)
        {
            if (id == 0) // new car - empty fields
            {
                ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "Name");
                ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "Type");
                ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name");
                return View(new Car());
            }
            else //edit a car
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
            
            Owner owner = _context.Owners.Find(car.OwnerId);
              if(car.CarId == 0) //if the car is new
              {
                if(!owner.Name.Equals("No owner")) // if the new car has a owner
                {
                owner.CarsBought++;
                _context.Update(owner);
                }
                car.Price -= (car.Price * car.Discount)/100;
                _context.Add(car);

            }
            else // if the car isn't new (edit)
            {
                if (!owner.Name.Equals("No owner")) //if the car has a owner
                {
                    owner.CarsBought++; //check to see if this is regarding the new owner or the old one
                    _context.Update(owner);
                }
                car.Price -= (car.Price * car.Discount)/100;
                _context.Update(car);
            }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
         
        }

   
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
            Owner owner = _context.Owners.Find(car.OwnerId);
            _context.Cars.Remove(car);

            if (!owner.Name.Equals("No owner"))
            {
                owner.CarsBought--;
                _context.Update(owner);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
