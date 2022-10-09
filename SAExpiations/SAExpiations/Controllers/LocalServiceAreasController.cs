using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        public async Task<IActionResult> Index(LocationExpiationCounter? userSearch, ExpiationDetails? vm)
        {
            var year = vm.selectedYear;
            // get number of expiations per location
            var query = _context.LocalServiceAreas.Select(p => new LocationExpiationCounter
            {
                LocalServiceAreaCode = p.LocalServiceAreaCode,
                LocalServiceArea1 = p.LocalServiceArea1,
                NumberofExpiations = _context.Expiations.Where(offence => offence.LocalServiceAreaCode == p.LocalServiceAreaCode && offence.IssueDate.Year == year).Count()
            }).OrderBy(e => e.LocalServiceArea1);

            // check if userSearch is empty
            if (!string.IsNullOrWhiteSpace(userSearch.SearchText))
            {

                // get number of expiations per location
                var query2 = query.Where(e => e.LocalServiceArea1.StartsWith(userSearch.SearchText)).OrderBy(e => e.LocalServiceArea1);

                return View(await query2.ToListAsync());
            }
            else {

                return View(await query.ToListAsync());

            }

        }

        // GET: LocalServiceAreas/Details/5
        public async Task<IActionResult> Details(string id, LocationServicesDetails? vm)
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

            // selected year
            var year = 2021;


            //var query = _context.Expiations.Where(e => e.IssueDate.Year == year & e.LocalServiceAreaCode == id).GroupBy(e => new
            //{
            //    SelectedArea = e.LocalServiceAreaCode,
            //    SelectedYear = year,
            //    ExpiationCode = e.ExpiationOffenceCode,
            //    ExpiationDescription = e.NoticeStatusDesc

            //}).Select(a => new LocationServicesDetails
            //{
            //    SelectedArea = a.Key.SelectedArea,
            //    SelectedYear = a.Key.SelectedYear,
            //    ExpiationCode = a.Key.ExpiationCode,
            //    ExpiationDescription = a.Key.ExpiationDescription,
            //    TotalExpiations = a.Count()


            //}).OrderBy(e => e.ExpiationCode);



            // selecting locations and expiations 
            var selectExpiationByLocation = (from location in _context.LocalServiceAreas
                                             where location.LocalServiceAreaCode == id
                                             join expiation in _context.Expiations on location.LocalServiceAreaCode equals expiation.LocalServiceAreaCode
                                             select new {
                                                 selectedAreaCode = location.LocalServiceAreaCode.ToString(),
                                                 SelectedArea = location.LocalServiceArea1,
                                                 ExpiationCode = expiation.ExpiationOffenceCode,
                                                 SelectedYear = expiation.IssueDate.Year

                                             });


            var locationDetails2 = (from a in selectExpiationByLocation
                                    where a.SelectedYear == year
                                   join b in _context.ExpiationOffences
                                   on a.ExpiationCode equals b.ExpiationOffenceCode
                                   select new {
                                       selectedAreaCode = a.selectedAreaCode,
                                       SelectedArea = a.SelectedArea,
                                       SelectedYear = a.SelectedYear,
                                       ExpiationCode = a.ExpiationCode,
                                       ExpiationDescription = b.ExpiationOffenceDescription
                                   });
            //var query3 = from p in locationDetails2
            //             group p by p.ExpiationCode 
            //             into g
            //            select new { g.Key, Count = g.Count() };

            var query4 = locationDetails2.GroupBy(x => new { 
                ExpiationCode = x.ExpiationCode,

                ExpiationDescription = x.ExpiationDescription }).Select(z => new LocationServicesDetails
                {
                    ExpiationCode = z.Key.ExpiationCode,
                    ExpiationDescription = z.Key.ExpiationDescription,
                    TotalExpiations = z.Count(),
                    SelectedYear = year,
                    SelectedArea = id
                });


           var result = await query4.ToArrayAsync();
            

            return View(result);
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
