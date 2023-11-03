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
    public class mainlinesController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public mainlinesController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: mainlines
        public async Task<IActionResult> Index()
        {
              return _context.mainline != null ? 
                          View(await _context.mainline.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.mainline'  is null.");
        }

        // GET: mainlines/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.mainline == null)
            {
                return NotFound();
            }

            var mainline = await _context.mainline
                .FirstOrDefaultAsync(m => m.id == id);
            if (mainline == null)
            {
                return NotFound();
            }

            return View(mainline);
        }

        // GET: mainlines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: mainlines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,viaduct_id,direction,avg_length,StartNode,EndNode")] mainline mainline)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mainline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mainline);
        }

        // GET: mainlines/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.mainline == null)
            {
                return NotFound();
            }

            var mainline = await _context.mainline.FindAsync(id);
            if (mainline == null)
            {
                return NotFound();
            }
            return View(mainline);
        }

        // POST: mainlines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,viaduct_id,direction,avg_length,StartNode,EndNode")] mainline mainline)
        {
            if (id != mainline.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mainlineExists(mainline.id))
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
            return View(mainline);
        }

        // GET: mainlines/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.mainline == null)
            {
                return NotFound();
            }

            var mainline = await _context.mainline
                .FirstOrDefaultAsync(m => m.id == id);
            if (mainline == null)
            {
                return NotFound();
            }

            return View(mainline);
        }

        // POST: mainlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.mainline == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.mainline'  is null.");
            }
            var mainline = await _context.mainline.FindAsync(id);
            if (mainline != null)
            {
                _context.mainline.Remove(mainline);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool mainlineExists(string id)
        {
          return (_context.mainline?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
