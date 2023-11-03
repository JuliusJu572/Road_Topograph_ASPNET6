using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoadAppWEB.Data;

namespace RoadAppWEB.Controllers
{
    public class AllNodesResController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public AllNodesResController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: AllNodesRes
        public async Task<IActionResult> Index()
        {
              return _context.AllNodesRes != null ? 
                          View(await _context.AllNodesRes.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.AllNodesRes'  is null.");
        }

        // GET: AllNodesRes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.AllNodesRes == null)
            {
                return NotFound();
            }

            var allNodesRes = await _context.AllNodesRes
                .FirstOrDefaultAsync(m => m.id == id);
            if (allNodesRes == null)
            {
                return NotFound();
            }

            return View(allNodesRes);
        }

        // GET: AllNodesRes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AllNodesRes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,node_id,hub_id,span,span2hub,direction,type")] AllNodesRes allNodesRes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allNodesRes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(allNodesRes);
        }

        // GET: AllNodesRes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.AllNodesRes == null)
            {
                return NotFound();
            }

            var allNodesRes = await _context.AllNodesRes.FindAsync(id);
            if (allNodesRes == null)
            {
                return NotFound();
            }
            return View(allNodesRes);
        }

        // POST: AllNodesRes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,node_id,hub_id,span,span2hub,direction,type")] AllNodesRes allNodesRes)
        {
            if (id != allNodesRes.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allNodesRes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllNodesResExists(allNodesRes.id))
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
            return View(allNodesRes);
        }

        // GET: AllNodesRes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.AllNodesRes == null)
            {
                return NotFound();
            }

            var allNodesRes = await _context.AllNodesRes
                .FirstOrDefaultAsync(m => m.id == id);
            if (allNodesRes == null)
            {
                return NotFound();
            }

            return View(allNodesRes);
        }

        // POST: AllNodesRes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AllNodesRes == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.AllNodesRes'  is null.");
            }
            var allNodesRes = await _context.AllNodesRes.FindAsync(id);
            if (allNodesRes != null)
            {
                _context.AllNodesRes.Remove(allNodesRes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllNodesResExists(int id)
        {
          return (_context.AllNodesRes?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
