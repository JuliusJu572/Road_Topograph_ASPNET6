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
    public class facilitiesController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public facilitiesController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: facilities
        public async Task<IActionResult> Index()
        {
              return _context.facility != null ? 
                          View(await _context.facility.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.facility'  is null.");
        }

        // GET: facilities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.facility == null)
            {
                return NotFound();
            }

            var facility = await _context.facility
                .FirstOrDefaultAsync(m => m.id == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // GET: facilities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,type,trunknetwork,maintenanceunit,operatingunit,lanes,length,square,date")] facility facility)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        // GET: facilities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.facility == null)
            {
                return NotFound();
            }

            var facility = await _context.facility.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }
            return View(facility);
        }

        // POST: facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,type,trunknetwork,maintenanceunit,operatingunit,lanes,length,square,date")] facility facility)
        {
            if (id != facility.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!facilityExists(facility.id))
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
            return View(facility);
        }

        // GET: facilities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.facility == null)
            {
                return NotFound();
            }

            var facility = await _context.facility
                .FirstOrDefaultAsync(m => m.id == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // POST: facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.facility == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.facility'  is null.");
            }
            var facility = await _context.facility.FindAsync(id);
            if (facility != null)
            {
                _context.facility.Remove(facility);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool facilityExists(string id)
        {
          return (_context.facility?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
