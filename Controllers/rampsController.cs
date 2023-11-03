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
    public class rampsController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public rampsController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: ramps
        public async Task<IActionResult> Index()
        {
              return _context.ramp != null ? 
                          View(await _context.ramp.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.ramp'  is null.");
        }

        // GET: ramps/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ramp == null)
            {
                return NotFound();
            }

            var ramp = await _context.ramp
                .FirstOrDefaultAsync(m => m.id == id);
            if (ramp == null)
            {
                return NotFound();
            }

            return View(ramp);
        }

        // GET: ramps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ramps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,viaduct_id,overpass_id,direction,code,start_node,end_node,linknode,avg_length")] ramp ramp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ramp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ramp);
        }

        // GET: ramps/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ramp == null)
            {
                return NotFound();
            }

            var ramp = await _context.ramp.FindAsync(id);
            if (ramp == null)
            {
                return NotFound();
            }
            return View(ramp);
        }

        // POST: ramps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,viaduct_id,overpass_id,direction,code,start_node,end_node,linknode,avg_length")] ramp ramp)
        {
            if (id != ramp.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ramp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!rampExists(ramp.id))
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
            return View(ramp);
        }

        // GET: ramps/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ramp == null)
            {
                return NotFound();
            }

            var ramp = await _context.ramp
                .FirstOrDefaultAsync(m => m.id == id);
            if (ramp == null)
            {
                return NotFound();
            }

            return View(ramp);
        }

        // POST: ramps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ramp == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.ramp'  is null.");
            }
            var ramp = await _context.ramp.FindAsync(id);
            if (ramp != null)
            {
                _context.ramp.Remove(ramp);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool rampExists(string id)
        {
          return (_context.ramp?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
