using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAExpiations.Data;
using SAExpiations.Models;
using SAExpiations.ViewModels;

namespace SAExpiations.Controllers
{
    public class LocalServiceAreasController : Controller
    {
        private readonly ExpiationsContext _context;

        public LocalServiceAreasController(ExpiationsContext context)
        {
            _context = context;
        }

        // GET: LocalServiceAreas
        public async Task<IActionResult> Index()
        {
            // get number of expiations per location
            var query = _context.LocalServiceAreas.Select(p => new LocationExpiationCounter {
            LocalServiceAreaCode = p.LocalServiceAreaCode,
                LocalServiceArea1 = p.LocalServiceArea1,
                NumberofExpiations = _context.Expiations.Where(offence => offence.LocalServiceAreaCode == p.LocalServiceAreaCode).Count()
            });
            
            
            return View(await query.ToListAsync());
        }

        // GET: LocalServiceAreas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.LocalServiceAreas == null)
            {
                return NotFound();
            }

            var localServiceArea = await _context.LocalServiceAreas
                .FirstOrDefaultAsync(m => m.LocalServiceAreaCode == id);
            if (localServiceArea == null)
            {
                return NotFound();
            }

            return View(localServiceArea);
        }

        // GET: LocalServiceAreas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocalServiceAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalServiceAreaCode,LocalServiceArea1")] LocalServiceArea localServiceArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localServiceArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localServiceArea);
        }

        // GET: LocalServiceAreas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.LocalServiceAreas == null)
            {
                return NotFound();
            }

            var localServiceArea = await _context.LocalServiceAreas.FindAsync(id);
            if (localServiceArea == null)
            {
                return NotFound();
            }
            return View(localServiceArea);
        }

        // POST: LocalServiceAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LocalServiceAreaCode,LocalServiceArea1")] LocalServiceArea localServiceArea)
        {
            if (id != localServiceArea.LocalServiceAreaCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localServiceArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalServiceAreaExists(localServiceArea.LocalServiceAreaCode))
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
            return View(localServiceArea);
        }

        // GET: LocalServiceAreas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.LocalServiceAreas == null)
            {
                return NotFound();
            }

            var localServiceArea = await _context.LocalServiceAreas
                .FirstOrDefaultAsync(m => m.LocalServiceAreaCode == id);
            if (localServiceArea == null)
            {
                return NotFound();
            }

            return View(localServiceArea);
        }

        // POST: LocalServiceAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.LocalServiceAreas == null)
            {
                return Problem("Entity set 'ExpiationsContext.LocalServiceAreas'  is null.");
            }
            var localServiceArea = await _context.LocalServiceAreas.FindAsync(id);
            if (localServiceArea != null)
            {
                _context.LocalServiceAreas.Remove(localServiceArea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalServiceAreaExists(string id)
        {
          return _context.LocalServiceAreas.Any(e => e.LocalServiceAreaCode == id);
        }
    }
}
