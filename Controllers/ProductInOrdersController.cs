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
    public class ProductInOrdersController : Controller
    {
        private readonly OnlineShopContext _context;

        public ProductInOrdersController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: ProductInOrders
        public async Task<IActionResult> Index(int? orderId)
        {
            var onlineShopContext = from po in _context.ProductInOrders select po;

            if( orderId != null )
            {
                onlineShopContext = onlineShopContext.Where(po => po.Orderid == orderId);
            }

            return View(await onlineShopContext.Include(p => p.Order).Include(p => p.Product).ToListAsync());
        }

        // GET: ProductInOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInOrder = await _context.ProductInOrders
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productInOrder == null)
            {
                return NotFound();
            }

            return View(productInOrder);
        }

        // GET: ProductInOrders/Create
        public IActionResult Create()
        {
            ViewData["Orderid"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["Productid"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: ProductInOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Productid,Amount,Orderid")] ProductInOrder productInOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productInOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Orderid"] = new SelectList(_context.Orders, "Id", "Id", productInOrder.Orderid);
            ViewData["Productid"] = new SelectList(_context.Products, "Id", "Id", productInOrder.Productid);
            return View(productInOrder);
        }

        // GET: ProductInOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInOrder = await _context.ProductInOrders.FindAsync(id);
            if (productInOrder == null)
            {
                return NotFound();
            }
            ViewData["Orderid"] = new SelectList(_context.Orders, "Id", "Id", productInOrder.Orderid);
            ViewData["Productid"] = new SelectList(_context.Products, "Id", "Id", productInOrder.Productid);
            return View(productInOrder);
        }

        // POST: ProductInOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Productid,Amount,Orderid")] ProductInOrder productInOrder)
        {
            if (id != productInOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInOrderExists(productInOrder.Id))
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
            ViewData["Orderid"] = new SelectList(_context.Orders, "Id", "Id", productInOrder.Orderid);
            ViewData["Productid"] = new SelectList(_context.Products, "Id", "Id", productInOrder.Productid);
            return View(productInOrder);
        }

        // GET: ProductInOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInOrder = await _context.ProductInOrders
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productInOrder == null)
            {
                return NotFound();
            }

            return View(productInOrder);
        }

        // POST: ProductInOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInOrder = await _context.ProductInOrders.FindAsync(id);
            if (productInOrder != null)
            {
                _context.ProductInOrders.Remove(productInOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInOrderExists(int id)
        {
            return _context.ProductInOrders.Any(e => e.Id == id);
        }
    }
}
