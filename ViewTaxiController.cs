
using AmneetTest2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmneetTest2.Controllers
{
    [Authorize]
    public class ViewTaxiController : Controller
    {
        
        private readonly TaxiDbContext context;

        public ViewTaxiController(TaxiDbContext context)
        {
            this.context = context;
        }
        // GET: ViewTaxiController
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {
            var availableTaxis = context.Taxis
                .Where(t => t.IsAvailable) // Filter only available taxis
                .AsQueryable(); // Convert to IQueryable
            var paginatedTaxis = await PaginatedList<Taxi>.CreateAsync(availableTaxis, pageNumber, pageSize);
            return View(paginatedTaxis);
        }


        // GET: ViewTaxiController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ViewTaxiController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViewTaxiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ViewTaxiController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ViewTaxiController/Edit/5
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

        // GET: ViewTaxiController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ViewTaxiController/Delete/5
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

