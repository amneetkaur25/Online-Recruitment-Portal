using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmneetTest2.Models;
using Microsoft.AspNetCore.Authorization;

namespace AmneetTest2.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly TaxiDbContext _context;

        public BookingsController(TaxiDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
              return _context.Bookings != null ? 
                          View(await _context.Bookings.ToListAsync()) :
                          Problem("Entity set 'TaxiDbContext.Bookings'  is null.");
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create(int? taxiId)
        {
            if (taxiId == null)
            {
                // Handle the case where TaxiId is missing
                return RedirectToAction(nameof(Index));
            }

            // Create a new Booking object with the provided TaxiId
            var booking = new Booking
            {
                TaxiId = (int)taxiId
            };

            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var selectedTaxi = await _context.Taxis.FirstOrDefaultAsync(t => t.Id == booking.TaxiId);

                if (selectedTaxi != null && selectedTaxi.IsAvailable)
                {
                    // Set the taxi as unavailable
                    selectedTaxi.IsAvailable = false;
                    _context.Update(selectedTaxi);

                    // Add the booking to the database
                    _context.Add(booking);
                    await _context.SaveChangesAsync();

                    // Display a success message
                    TempData["SuccessMessage"] = "Taxi booked successfully.";

                    // Redirect back to the "Available Taxis" page
                    return RedirectToAction("AvailableTaxis", "Bookings");
                }
                else
                {
                    ModelState.AddModelError("", "Selected taxi is not available for booking.");
                }
            }

            return View(booking);
        }



        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,CustomerName,PickupLocation,DropOffLocation,BookingTime")] Booking booking)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(booking);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(booking);
        //}



        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerName,PickupLocation,DropOffLocation,BookingTime")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        
        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null || _context.Taxis == null)
            {
                return Problem("Entity set 'TaxiDbContext.Bookings' or 'TaxiDbContext.Taxis' is null.");
            }

            var booking = await _context.Bookings.FindAsync(id);

            if (booking != null)
            {
                // Find the associated taxi
                var taxi = await _context.Taxis.FindAsync(booking.TaxiId);

                if (taxi != null)
                {
                    // Mark the taxi as available
                    taxi.IsAvailable = true;

                    // Remove the booking
                    _context.Bookings.Remove(booking);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Booking deleted successfully.";
                }
            }

            return RedirectToAction("Index","ViewBookings");
        
        }




        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> AvailableTaxis(int pageNumber = 1, int pageSize = 2)
        {
            // Retrieve available taxis as IQueryable
            var query = _context.Taxis.Where(t => t.IsAvailable).AsQueryable();

            // Create a paginated list of available taxis
            var paginatedTaxis = await PaginatedList<Taxi>.CreateAsync(query, pageNumber, pageSize);

            return View(paginatedTaxis);
        }
    }
}
