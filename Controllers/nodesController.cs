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
    public class nodesController : Controller
    {
        private readonly RoadAppWEBContext _context;

        public nodesController(RoadAppWEBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: nodes
        public async Task<IActionResult> Index(string searchString)
        {
            var nodes = from n in _context.node
                         select n;
            if (!String.IsNullOrEmpty(searchString))
            {
                nodes = nodes.Where(s => s.id!.Contains(searchString));
            }
            return View(await nodes.ToListAsync());
        }

        // GET: nodes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.node == null)
            {
                return NotFound();
            }

            var node = await _context.node
                .FirstOrDefaultAsync(m => m.id == id);
            if (node == null)
            {
                return NotFound();
            }

            return View(node);
        }

        // GET: nodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: nodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,fathernode,childnode,mainline_id,ramp_id,level,longitude,latitude")] node node)
        {
            if (ModelState.IsValid)
            {
                _context.Add(node);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(node);
        }

        // GET: nodes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.node == null)
            {
                return NotFound();
            }
            
            var node = await _context.node.FindAsync(id);
            if (node == null)
            {
                return NotFound();
            }
            return View(node);
        }

        // POST: nodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,fathernode,childnode,mainline_id,ramp_id,level,longitude,latitude")] node node)
        {
            if (id != node.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(node);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!nodeExists(node.id))
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
            return View(node);
        }

        // GET: nodes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.node == null)
            {
                return NotFound();
            }

            var node = await _context.node
                .FirstOrDefaultAsync(m => m.id == id);
            if (node == null)
            {
                return NotFound();
            }

            return View(node);
        }

        // POST: nodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.node == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.node'  is null.");
            }
            var node = await _context.node.FindAsync(id);
            if (node != null)
            {
                _context.node.Remove(node);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool nodeExists(string id)
        {
          return (_context.node?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
