using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class WorkersController : Controller
    {
        private readonly OnlineShopContext _context;

        public WorkersController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: Workers
        public async Task<IActionResult> Index(int? PickUpPointId)
        {
            var onlineShopContext = from p in _context.Workers select p;

            if(PickUpPointId != null)
            {
                onlineShopContext = onlineShopContext.Where(p => p.PickupPoint == PickUpPointId);
            }

            return View(await onlineShopContext.Include(w => w.Jobtitle).Include(w => w.PickupPointNavigation).ToListAsync());
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(w => w.Jobtitle)
                .Include(w => w.PickupPointNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            ViewData["Jobtitleid"] = new SelectList(_context.Jobtitles, "Id", "Id");
            ViewData["PickupPoint"] = new SelectList(_context.PickupPoints, "Id", "Id");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Salary,Jobtitleid,PickupPoint,Login,Password,IsEmployeed")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Jobtitleid"] = new SelectList(_context.Jobtitles, "Id", "Id", worker.Jobtitleid);
            ViewData["PickupPoint"] = new SelectList(_context.PickupPoints, "Id", "Id", worker.PickupPoint);
            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            ViewData["Jobtitleid"] = new SelectList(_context.Jobtitles, "Id", "Id", worker.Jobtitleid);
            ViewData["PickupPoint"] = new SelectList(_context.PickupPoints, "Id", "Id", worker.PickupPoint);
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Salary,Jobtitleid,PickupPoint,Login,Password,IsEmployeed")] Worker worker)
        {
            if (id != worker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.Id))
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
            ViewData["Jobtitleid"] = new SelectList(_context.Jobtitles, "Id", "Id", worker.Jobtitleid);
            ViewData["PickupPoint"] = new SelectList(_context.PickupPoints, "Id", "Id", worker.PickupPoint);
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(w => w.Jobtitle)
                .Include(w => w.PickupPointNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null)
            {
                _context.Workers.Remove(worker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }
    }
}
