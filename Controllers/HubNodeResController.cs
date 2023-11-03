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
    public class HubNodeResController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public HubNodeResController(RoadAppWEBContext context)
        {
            _context = context;
        }

        // GET: HubNodeRes
        public async Task<IActionResult> Index()
        {
              return _context.HubNodeRes != null ? 
                          View(await _context.HubNodeRes.ToListAsync()) :
                          Problem("Entity set 'RoadAppWEBContext.HubNodeRes'  is null.");
        }

        // GET: HubNodeRes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.HubNodeRes == null)
            {
                return NotFound();
            }

            var hubNodeRes = await _context.HubNodeRes
                .FirstOrDefaultAsync(m => m.id == id);
            if (hubNodeRes == null)
            {
                return NotFound();
            }

            return View(hubNodeRes);
        }

        // GET: HubNodeRes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HubNodeRes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,starthub_id,endhub_id,span,velocity,direction,type")] HubNodeRes hubNodeRes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hubNodeRes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hubNodeRes);
        }

        // GET: HubNodeRes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.HubNodeRes == null)
            {
                return NotFound();
            }

            var hubNodeRes = await _context.HubNodeRes.FindAsync(id);
            if (hubNodeRes == null)
            {
                return NotFound();
            }
            return View(hubNodeRes);
        }

        // POST: HubNodeRes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,starthub_id,endhub_id,span,velocity,direction,type")] HubNodeRes hubNodeRes)
        {
            if (id != hubNodeRes.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hubNodeRes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HubNodeResExists(hubNodeRes.id))
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
            return View(hubNodeRes);
        }

        // GET: HubNodeRes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.HubNodeRes == null)
            {
                return NotFound();
            }

            var hubNodeRes = await _context.HubNodeRes
                .FirstOrDefaultAsync(m => m.id == id);
            if (hubNodeRes == null)
            {
                return NotFound();
            }

            return View(hubNodeRes);
        }

        // POST: HubNodeRes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.HubNodeRes == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.HubNodeRes'  is null.");
            }
            var hubNodeRes = await _context.HubNodeRes.FindAsync(id);
            if (hubNodeRes != null)
            {
                _context.HubNodeRes.Remove(hubNodeRes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HubNodeResExists(int id)
        {
          return (_context.HubNodeRes?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
