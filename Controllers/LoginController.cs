using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

        //View login page
        // GET: Login
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("UID", -1);
            HttpContext.Session.SetString("UType", "");
            return View();
        }

        //Login process
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginMember([Bind("UserEmail,UserPass")] TUser tUser)
        {
            try
            {
                tUser.ShaEnc();
                var user = await _context.TUsers
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

                if ((bool)user.UserType) 
                { 
                    return RedirectToAction("Index", "Home"); 
                }
                else return RedirectToAction("Index", "Admin");
            }
            catch (Exception)
            {
                TempData["Error"] = "Error occured during login process. Please try again.";
                return RedirectToAction("Index");
            }
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

        private bool TUserExists(int id)
        {
            return _context.TUsers.Any(e => e.UserId == id);
        }
    }
}
