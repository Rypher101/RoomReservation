using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;
using RoomReservation.Models;

namespace RoomReservation.Controllers
{
    public class TCategoriesController : Controller
    {
        private readonly RoomReservationContext _context;

        public TCategoriesController(RoomReservationContext context)
        {
            _context = context;
        }

        // GET: TCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.TCategory.ToListAsync());
        }

        // GET: TCategories/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tCategory = await _context.TCategory
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (tCategory == null)
            {
                return NotFound();
            }

            return View(tCategory);
        }

        // GET: TCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatId,CatType,CatBed,CatDescription,CatPrice")] TCategory tCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tCategory);
        }

        // GET: TCategories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tCategory = await _context.TCategory.FindAsync(id);
            if (tCategory == null)
            {
                return NotFound();
            }
            return View(tCategory);
        }

        // POST: TCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CatId,CatType,CatBed,CatDescription,CatPrice")] TCategory tCategory)
        {
            if (id != tCategory.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TCategoryExists(tCategory.CatId))
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
            return View(tCategory);
        }

        // GET: TCategories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tCategory = await _context.TCategory
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (tCategory == null)
            {
                return NotFound();
            }

            return View(tCategory);
        }

        // POST: TCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tCategory = await _context.TCategory.FindAsync(id);
            _context.TCategory.Remove(tCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TCategoryExists(string id)
        {
            return _context.TCategory.Any(e => e.CatId == id);
        }
    }
}
