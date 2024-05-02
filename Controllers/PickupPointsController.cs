using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]

    public class PickupPointsController : Controller
    {
        private readonly OnlineShopContext _context;

        public PickupPointsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: PickupPoints
        public async Task<IActionResult> Index()
        {
            return View(await _context.PickupPoints.ToListAsync());
        }

        // GET: PickupPoints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickupPoint = await _context.PickupPoints
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pickupPoint == null)
            {
                return NotFound();
            }

            return View(pickupPoint);
        }

        // GET: PickupPoints/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PickupPoints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Adress,Rating")] PickupPoint pickupPoint)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pickupPoint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pickupPoint);
        }

        // GET: PickupPoints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickupPoint = await _context.PickupPoints.FindAsync(id);
            if (pickupPoint == null)
            {
                return NotFound();
            }
            return View(pickupPoint);
        }

        // POST: PickupPoints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adress,Rating")] PickupPoint pickupPoint)
        {
            if (id != pickupPoint.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pickupPoint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PickupPointExists(pickupPoint.Id))
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
            return View(pickupPoint);
        }

        // GET: PickupPoints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickupPoint = await _context.PickupPoints
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pickupPoint == null)
            {
                return NotFound();
            }

            return View(pickupPoint);
        }

        // POST: PickupPoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pickupPoint = await _context.PickupPoints.FindAsync(id);
            if (pickupPoint != null)
            {
                _context.PickupPoints.Remove(pickupPoint);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PickupPointExists(int id)
        {
            return _context.PickupPoints.Any(e => e.Id == id);
        }
    }
}
