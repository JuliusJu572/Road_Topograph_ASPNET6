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
    public class viaductsController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public viaductsController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: viaducts
        public async Task<IActionResult> Index()
        {
              return _context.viaduct != null ? 
                          View(await _context.viaduct.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.viaduct'  is null.");
        }

        // GET: viaducts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.viaduct == null)
            {
                return NotFound();
            }

            var viaduct = await _context.viaduct
                .FirstOrDefaultAsync(m => m.id == id);
            if (viaduct == null)
            {
                return NotFound();
            }

            return View(viaduct);
        }

        // GET: viaducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: viaducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,trunknetwork,maintenanceunit,operatingunit,lanes,length,square,date")] viaduct viaduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viaduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viaduct);
        }

        // GET: viaducts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.viaduct == null)
            {
                return NotFound();
            }

            var viaduct = await _context.viaduct.FindAsync(id);
            if (viaduct == null)
            {
                return NotFound();
            }
            return View(viaduct);
        }

        // POST: viaducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,trunknetwork,maintenanceunit,operatingunit,lanes,length,square,date")] viaduct viaduct)
        {
            if (id != viaduct.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viaduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!viaductExists(viaduct.id))
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
            return View(viaduct);
        }

        // GET: viaducts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.viaduct == null)
            {
                return NotFound();
            }

            var viaduct = await _context.viaduct
                .FirstOrDefaultAsync(m => m.id == id);
            if (viaduct == null)
            {
                return NotFound();
            }

            return View(viaduct);
        }

        // POST: viaducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.viaduct == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.viaduct'  is null.");
            }
            var viaduct = await _context.viaduct.FindAsync(id);
            if (viaduct != null)
            {
                _context.viaduct.Remove(viaduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool viaductExists(string id)
        {
          return (_context.viaduct?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
