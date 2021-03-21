using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoomReservationContext _context;

        public AdminController(RoomReservationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }

            SetDashboard(1);
            return View();
        }

        public IActionResult ViewCategory()
        {
            SetDashboard(2);
            var tCat = _context.TCategory.ToList();
            return View(tCat);
        }

        public IActionResult NewCategory()
        {
            SetDashboard(2);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewCategory([Bind("CatId,CatType,CatBed,CatDescription,CatPrice")] TCategory tCat)
        {
            SetDashboard(2);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(tCat);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Category registered.";
                    return RedirectToAction(nameof(ViewCategory));
                }
                catch (Exception)
                {
                    TempData["Error"] = "Unable to register this category. Please try agian";
                    return View(tCat);

                }

            }

            TempData["Error"] = "Unable to register this category. Please try agian";
            return View(tCat);
        }

        public async Task<IActionResult> EditCategory(string id)
        {
            SetDashboard(2);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(string id, [Bind("CatId,CatType,CatBed,CatDescription,CatPrice")] TCategory tCategory)
        {
            SetDashboard(2);
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
                    if (!_context.TCategory.Any(e => e.CatId == id))
                    {
                        TempData["Error"] = "Couldn't find the category ID. Please try again";
                        return RedirectToAction(nameof(ViewCategory));
                    }
                    else
                    {
                        TempData["Error"] = "Couldn't update the category. Please try again";
                        return RedirectToAction(nameof(ViewCategory));
                    }
                }
                TempData["Message"] = "Update successfull.";
                return RedirectToAction(nameof(ViewCategory));
            }

            TempData["Error"] = "Couldn't update the category. Please try again";
            return View(tCategory);
        }

        // GET: TCategories/Delete/5
        public async Task<IActionResult> DeleteCategory(string id)
        {
            SetDashboard(2);
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
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(string id)
        {
            SetDashboard(2);
            var tCategory = await _context.TCategory.FindAsync(id);
            _context.TCategory.Remove(tCategory);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Delete successfull.";
            return RedirectToAction(nameof(ViewCategory));
        }

        private void SetDashboard(int value)
        {
            switch (value)
            {
                case 1:
                    TempData["Dashboard"] = "active";
                    break;
                case 2:
                    TempData["Category"] = "active";
                    break;
                case 3:
                    TempData["Rooms"] = "active";
                    break;
                case 4:
                    TempData["Users"] = "active";
                    break;
                default:
                    break;
            }
        }
    }
}
