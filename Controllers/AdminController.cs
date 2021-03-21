using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UType") != "A") {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login"); 
            }
            return View();
        }

        public void SetDashboard(int value)
        {
            switch (value)
            {

                default:
                    break;
            }
        }
    }
}
