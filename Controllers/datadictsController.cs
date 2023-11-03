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
    public class datadictsController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public datadictsController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: datadicts
        public async Task<IActionResult> Index()
        {
              return _context.datadict != null ? 
                          View(await _context.datadict.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.datadict'  is null.");
        }

        // GET: datadicts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.datadict == null)
            {
                return NotFound();
            }

            var datadict = await _context.datadict
                .FirstOrDefaultAsync(m => m.id == id);
            if (datadict == null)
            {
                return NotFound();
            }

            return View(datadict);
        }

        // GET: datadicts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: datadicts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,reference,content")] datadict datadict)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datadict);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(datadict);
        }

        // GET: datadicts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.datadict == null)
            {
                return NotFound();
            }

            var datadict = await _context.datadict.FindAsync(id);
            if (datadict == null)
            {
                return NotFound();
            }
            return View(datadict);
        }

        // POST: datadicts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,reference,content")] datadict datadict)
        {
            if (id != datadict.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datadict);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!datadictExists(datadict.id))
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
            return View(datadict);
        }

        // GET: datadicts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.datadict == null)
            {
                return NotFound();
            }

            var datadict = await _context.datadict
                .FirstOrDefaultAsync(m => m.id == id);
            if (datadict == null)
            {
                return NotFound();
            }

            return View(datadict);
        }

        // POST: datadicts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.datadict == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.datadict'  is null.");
            }
            var datadict = await _context.datadict.FindAsync(id);
            if (datadict != null)
            {
                _context.datadict.Remove(datadict);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool datadictExists(int id)
        {
          return (_context.datadict?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
