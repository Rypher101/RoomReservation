using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;
using RoomReservation.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginMember([Bind("UserEmail,UserPass")] TUser tUser)
        {
            try
            {
                tUser.ShaEnc();
                var user = await _context.TUser
                    .FirstOrDefaultAsync(e => e.UserEmail == tUser.UserEmail && e.UserPass == tUser.UserPass && e.UserStatus == 1);

                if (user == null)
                {
                    TempData["Error"] = "Invalid login credentials";
                    return RedirectToAction("Index");
                }

                HttpContext.Session.SetString("UName", user.UserName);
                HttpContext.Session.SetString("UEmail", user.UserEmail);
                HttpContext.Session.SetInt32("UID", user.UserId);
                HttpContext.Session.SetString("UType", (bool)user.UserType ? "C" : "A");

                if ((bool)user.UserType) return RedirectToAction("Index", "Home");
                else return RedirectToAction("Index", "Admin");
            }
            catch (Exception)
            {
                TempData["Error"] = "Error occured during login process. Please try again.";
                return RedirectToAction("Index");
            }
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
                    tUser.UserStatus = 1;
                    tUser.ShaEnc();
                    _context.Add(tUser);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "User registered.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.ToString().Contains("Violation of UNIQUE KEY constraint")) ViewBag.Error = "This email is already used. Please use another email address";
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
