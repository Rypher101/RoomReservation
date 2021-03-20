using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Models;

namespace RoomReservation.Controllers
{
    public class LoginController : Controller
    {
        private readonly RoomReservationContext _context;

        public LoginController(RoomReservationContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUser = await _context.TUser
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (tUser == null)
            {
                return NotFound();
            }

            return View(tUser);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,UserPass,UserEmail,UserAddress,UserTp")] TUser tUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tUser.ShaEnc();
                    _context.Add(tUser);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "User registered.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if(ex.InnerException.ToString().Contains("Violation of UNIQUE KEY constraint")) ViewBag.Error = "This email is already used. Please use another email address";
                    else ViewBag.Error = "Unable to register this user. Please try agian";
                    return View(tUser);

                }
                
            }

            ViewBag.Error = "Unable to register this user. Please try agian";
            return View(tUser);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUser = await _context.TUser.FindAsync(id);
            if (tUser == null)
            {
                return NotFound();
            }
            return View(tUser);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,UserPass,UserEmail,UserAddress,UserTp,UserStatus")] TUser tUser)
        {
            if (id != tUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TUserExists(tUser.UserId))
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
            return View(tUser);
        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUser = await _context.TUser
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (tUser == null)
            {
                return NotFound();
            }

            return View(tUser);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tUser = await _context.TUser.FindAsync(id);
            _context.TUser.Remove(tUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TUserExists(int id)
        {
            return _context.TUser.Any(e => e.UserId == id);
        }
    }
}
