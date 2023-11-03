using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoadAppWEB.Data;
using RoadAppWEB.Models;

namespace RoadAppWEB.Controllers
{
    public class overpassesController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public overpassesController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: overpasses
        public async Task<IActionResult> Index()
        {
              return _context.overpass != null ? 
                          View(await _context.overpass.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.overpass'  is null.");
        }

        // GET: overpasses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.overpass == null)
            {
                return NotFound();
            }

            var overpass = await _context.overpass
                .FirstOrDefaultAsync(m => m.id == id);
            if (overpass == null)
            {
                return NotFound();
            }

            return View(overpass);
        }

        // GET: overpasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: overpasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,trunknetwork,maintenanceunit,operatingunit,lanes,length,square,date")] overpass overpass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(overpass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(overpass);
        }

        // GET: overpasses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.overpass == null)
            {
                return NotFound();
            }

            var overpass = await _context.overpass.FindAsync(id);
            if (overpass == null)
            {
                return NotFound();
            }
            return View(overpass);
        }

        // POST: overpasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,trunknetwork,maintenanceunit,operatingunit,lanes,length,square,date")] overpass overpass)
        {
            if (id != overpass.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(overpass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!overpassExists(overpass.id))
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
            return View(overpass);
        }

        // GET: overpasses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.overpass == null)
            {
                return NotFound();
            }

            var overpass = await _context.overpass
                .FirstOrDefaultAsync(m => m.id == id);
            if (overpass == null)
            {
                return NotFound();
            }

            return View(overpass);
        }

        // POST: overpasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.overpass == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.overpass'  is null.");
            }
            var overpass = await _context.overpass.FindAsync(id);
            if (overpass != null)
            {
                _context.overpass.Remove(overpass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool overpassExists(string id)
        {
          return (_context.overpass?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
