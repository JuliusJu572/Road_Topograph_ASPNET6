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
    public class roadsController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public roadsController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: roads
        public async Task<IActionResult> Index()
        {
              return _context.road != null ? 
                          View(await _context.road.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.road'  is null.");
        }

        // GET: roads/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.road == null)
            {
                return NotFound();
            }

            var road = await _context.road
                .FirstOrDefaultAsync(m => m.id == id);
            if (road == null)
            {
                return NotFound();
            }

            return View(road);
        }

        // GET: roads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: roads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,facility_id,type,direction,code,start_node,start_linknode,end_node,end_linknode,avg_length")] road road)
        {
            if (ModelState.IsValid)
            {
                _context.Add(road);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(road);
        }

        // GET: roads/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.road == null)
            {
                return NotFound();
            }

            var road = await _context.road.FindAsync(id);
            if (road == null)
            {
                return NotFound();
            }
            return View(road);
        }

        // POST: roads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,facility_id,type,direction,code,start_node,start_linknode,end_node,end_linknode,avg_length")] road road)
        {
            if (id != road.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(road);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!roadExists(road.id))
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
            return View(road);
        }

        // GET: roads/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.road == null)
            {
                return NotFound();
            }

            var road = await _context.road
                .FirstOrDefaultAsync(m => m.id == id);
            if (road == null)
            {
                return NotFound();
            }

            return View(road);
        }

        // POST: roads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.road == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.road'  is null.");
            }
            var road = await _context.road.FindAsync(id);
            if (road != null)
            {
                _context.road.Remove(road);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool roadExists(string id)
        {
          return (_context.road?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
