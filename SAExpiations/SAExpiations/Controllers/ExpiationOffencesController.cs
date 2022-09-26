using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SAExpiations.Data;
using SAExpiations.Models;
using SAExpiations.ViewModels;

namespace SAExpiations.Controllers
{
    public class ExpiationOffencesController : Controller
    {
        private readonly ExpiationsContext _context;

        public ExpiationOffencesController(ExpiationsContext context)
        {
            _context = context;
        }

        // GET: ExpiationOffences
        public async Task<IActionResult> Index(string? SearchText)
        {

            
            
            // only expiations with offence counts
            //var query = (from a in _context.ExpiationOffences
            //             join b in _context.Expiations
            //             on a.ExpiationOffenceCode equals b.ExpiationOffenceCode
            //             select new { ExpiationOffenceCode = a.ExpiationOffenceCode, ExpiationOffenceDescription = a.ExpiationOffenceDescription, ExpiationOffenceCodeCount = b.ExpiationOffenceCode }).GroupBy(offence => new { ExpiationOffenceCode = offence.ExpiationOffenceCode, ExpiationOffenceDescription = offence.ExpiationOffenceDescription })
            //    .Select(x => new ExpiationCounter { ExpiationOffenceCode = x.Key.ExpiationOffenceCode, ExpiationOffenceDescription = x.Key.ExpiationOffenceDescription, ExpiationCount = x.Count() });

            // show all expiations 
            var query2 = _context.ExpiationOffences.Select(p => new ExpiationCounter {
            ExpiationOffenceCode = p.ExpiationOffenceCode,
                ExpiationOffenceDescription = p.ExpiationOffenceDescription,
                ExpiationCount = _context.Expiations.Where(c => c.ExpiationOffenceCode == p.ExpiationOffenceCode).Count()
            });
            //var result = await _context.ExpiationOffences.ToListAsync();
            var result = query2.ToList();

            return View(result);
        }

        // GET: ExpiationOffences/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ExpiationOffences == null)
            {
                return NotFound();
            }

            var expiationOffence = await _context.ExpiationOffences
                .FirstOrDefaultAsync(m => m.ExpiationOffenceCode == id);
            if (expiationOffence == null)
            {
                return NotFound();
            }
            // join expiations database and the expiation offences database
            
            var year = 2021;
            // groupby
            var query2 = _context.Expiations.Where(e => e.ExpiationOffenceCode == id & e.IssueDate.Year == year).GroupBy(e => new { IssueDate = e.IssueDate.Month, NoticeStatusDesc = e.NoticeStatusDesc }).Select(y => new ExpiationDetails {

                NoticeStatusDesc = y.Key.NoticeStatusDesc,
                IssueDate = y.Key.IssueDate,
                StatusCount = y.Count(),
                ExpiationOffenceCode = id

            });

            var result = query2.ToList();

            return View(result);
        }

        // GET: ExpiationOffences/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpiationOffences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpiationOffenceCode,ExpiationOffenceDescription")] ExpiationOffence expiationOffence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expiationOffence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expiationOffence);
        }

        // GET: ExpiationOffences/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ExpiationOffences == null)
            {
                return NotFound();
            }

            var expiationOffence = await _context.ExpiationOffences.FindAsync(id);
            if (expiationOffence == null)
            {
                return NotFound();
            }
            return View(expiationOffence);
        }

        // POST: ExpiationOffences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ExpiationOffenceCode,ExpiationOffenceDescription")] ExpiationOffence expiationOffence)
        {
            if (id != expiationOffence.ExpiationOffenceCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expiationOffence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpiationOffenceExists(expiationOffence.ExpiationOffenceCode))
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
            return View(expiationOffence);
        }

        // GET: ExpiationOffences/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ExpiationOffences == null)
            {
                return NotFound();
            }

            var expiationOffence = await _context.ExpiationOffences
                .FirstOrDefaultAsync(m => m.ExpiationOffenceCode == id);
            if (expiationOffence == null)
            {
                return NotFound();
            }

            return View(expiationOffence);
        }

        // POST: ExpiationOffences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ExpiationOffences == null)
            {
                return Problem("Entity set 'ExpiationsContext.ExpiationOffences'  is null.");
            }
            var expiationOffence = await _context.ExpiationOffences.FindAsync(id);
            if (expiationOffence != null)
            {
                _context.ExpiationOffences.Remove(expiationOffence);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpiationOffenceExists(string id)
        {
          return (_context.ExpiationOffences?.Any(e => e.ExpiationOffenceCode == id)).GetValueOrDefault();
        }
    }
}
