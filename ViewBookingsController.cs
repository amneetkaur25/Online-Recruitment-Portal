using AmneetTest2.Models;
using AmneetTest2.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace AmneetTest2.Controllers
{
    [Authorize]
    public class ViewBookingsController : Controller
    {
        
        private readonly TaxiDbContext context;

        public ViewBookingsController(TaxiDbContext context)
        {
            this.context = context;
        }
        // GET: ViewBookingsController
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {
            var bookings = context.Bookings
             .Select(p => new BookingViewModel
             {
                 Id = p.Id,
                 CustomerName = p.CustomerName,
                 PickupLocation = p.PickupLocation,
                 DropOffLocation = p.DropOffLocation,
                 BookingTime = p.BookingTime,
                 // Retrieve related data for Taxi and map it to the view model properties
                 DriverName = context.Taxis.FirstOrDefault(t => t.Id == p.TaxiId).DriverName,
                 CarModel = context.Taxis.FirstOrDefault(t => t.Id == p.TaxiId).CarModel,
                 LicensePlate = context.Taxis.FirstOrDefault(t => t.Id == p.TaxiId).LicensePlate
             })
             .AsQueryable();
            var paginatedBookings = await PaginatedList<BookingViewModel>.CreateAsync(bookings, pageNumber, pageSize);
            return View(paginatedBookings);
            
            //    var bookings = context.Bookings.ToList();
            //    return View(bookings);
        }

        // GET: ViewBookingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

      


        // GET: ViewBookingsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ViewBookingsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ViewBookingsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ViewBookingsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       
    }
}
