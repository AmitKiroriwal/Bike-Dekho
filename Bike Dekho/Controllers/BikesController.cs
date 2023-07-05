using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bike_Dekho.Data;
using Bike_Dekho.Models;

namespace Bike_Dekho.Controllers
{
    public class BikesController : Controller
    {
        private readonly AppDbContext _context;

        public BikesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Bikes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Bikes.Include(b => b.Make).Include(b => b.Model);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Bikes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bikes == null)
            {
                return NotFound();
            }

            var bikes = await _context.Bikes
                .Include(b => b.Make)
                .Include(b => b.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bikes == null)
            {
                return NotFound();
            }

            return View(bikes);
        }

        // GET: Bikes/Create
        public IActionResult Create()
        {
            ViewData["MakeId"] = new SelectList(_context.Makes, "Id", "Name");
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name");
            return View();
        }

        // POST: Bikes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MakeId,ModelId,Year,Mileage,Features,SellerName,SellerEmail,SellerPhone,Price,Currency,ImagePath")] Bikes bikes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bikes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.Makes, "Id", "Name", bikes.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", bikes.ModelId);
            return View(bikes);
        }

        // GET: Bikes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bikes == null)
            {
                return NotFound();
            }

            var bikes = await _context.Bikes.FindAsync(id);
            if (bikes == null)
            {
                return NotFound();
            }
            ViewData["MakeId"] = new SelectList(_context.Makes, "Id", "Name", bikes.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", bikes.ModelId);
            return View(bikes);
        }

        // POST: Bikes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MakeId,ModelId,Year,Mileage,Features,SellerName,SellerEmail,SellerPhone,Price,Currency,ImagePath")] Bikes bikes)
        {
            if (id != bikes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bikes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BikesExists(bikes.Id))
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
            ViewData["MakeId"] = new SelectList(_context.Makes, "Id", "Name", bikes.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", bikes.ModelId);
            return View(bikes);
        }

        // GET: Bikes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bikes == null)
            {
                return NotFound();
            }

            var bikes = await _context.Bikes
                .Include(b => b.Make)
                .Include(b => b.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bikes == null)
            {
                return NotFound();
            }

            return View(bikes);
        }

        // POST: Bikes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bikes == null)
            {
                return Problem("Entity set 'AppDbContext.Bikes'  is null.");
            }
            var bikes = await _context.Bikes.FindAsync(id);
            if (bikes != null)
            {
                _context.Bikes.Remove(bikes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BikesExists(int id)
        {
          return (_context.Bikes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
