using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RoomReservation.Data;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RoomReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoomReservationContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, RoomReservationContext context)
        {
            _context = context;
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            var cus = from rs in _context.TReservation
                      join rr in _context.TReservationRoom on rs.ResId equals rr.ResId
                      join rm in _context.TRoom on rr.RoomId equals rm.RoomId
                      join cat in _context.TCategory on rm.CatId equals cat.CatId
                      group cat by cat.CatId into bdCount
                      select new
                      {
                          customers = bdCount.Sum(x => x.CatBed)
                      };

            var reservations = (from res in _context.TReservation
                                where res.ResStatus == 1 && res.ResTo > DateTime.Today
                                select res).Count();
            return View();
        }

        public IActionResult Category()
        {
            var tCategory = _context.TCategory
                .Include(ic => ic.TImg)
                .ToList();

            ViewBag.Min = DateTime.Today.ToString("yyyy-MM-dd");

            HttpContext.Session.SetString("catID", "");
            if (HttpContext.Session.GetString("resFrom") != null)
            {
                ViewBag.From = DateTime.Parse(HttpContext.Session.GetString("resFrom"));
                ViewBag.To = DateTime.Parse(HttpContext.Session.GetString("resTo"));
                ViewBag.Prv = "display:none;";
            }

            return View(tCategory);
        }

        public IActionResult Rooms(decimal id = 0, IFormCollection collection = null)
        {
            DateTime dtFrom;
            DateTime dtTo;
            string catID;
            List<TRoom> roomList = new List<TRoom>();

            if (HttpContext.Session.GetString("rooms") != null)
            {
                roomList = JsonConvert.DeserializeObject<List<TRoom>>(HttpContext.Session.GetString("rooms"));
            }

            if (collection.Count > 0)
            {
                dtFrom = DateTime.Parse(collection["resFrom"]);
                dtTo = DateTime.Parse(collection["resTo"]);
                catID = collection["catID"];

                HttpContext.Session.SetString("resFrom", dtFrom.ToShortDateString());
                HttpContext.Session.SetString("resTo", dtTo.ToShortDateString());
                HttpContext.Session.SetString("resCat", catID);
            }
            else
            {
                dtFrom = DateTime.Parse(HttpContext.Session.GetString("resFrom"));
                dtTo = DateTime.Parse(HttpContext.Session.GetString("resTo"));
                catID = HttpContext.Session.GetString("resCat");

                var newRoom = new TRoom { RoomId = id, CatId = catID };
                if (HttpContext.Session.GetString("rooms") == null)
                {
                    roomList.Add(newRoom);
                    HttpContext.Session.SetString("rooms", JsonConvert.SerializeObject(roomList));
                    HttpContext.Session.SetInt32("pending", 1);
                }
                else
                {
                    roomList = JsonConvert.DeserializeObject<List<TRoom>>(HttpContext.Session.GetString("rooms"));
                    if (id != -1)
                    {
                        roomList.Add(newRoom);
                        HttpContext.Session.SetString("rooms", JsonConvert.SerializeObject(roomList));
                    }
                }
            }

            var tRoom = _context.TRoom
                .Where(x => x.CatId == catID.ToString() &&
                    !_context.TReservationRoom
                        .Include(res => res.Res)
                        .Where(res => res.Res.ResFrom >= dtFrom && res.Res.ResTo <= dtTo)
                        .Select(res => res.RoomId)
                    .Contains(x.RoomId) &&
                    x.RoomStatus == 1)
                .ToList();


                foreach (var item in tRoom)
                {
                    item.TRate = _context.TRate.Where(x => x.RoomId == item.RoomId).ToList();

                    foreach (var item2 in roomList)
                    {
                        if (item.RoomId == item2.RoomId)
                        {
                            item.RoomStatus = 0;
                            break;
                        }
                    }
                }

            return View(tRoom);
        }

        public IActionResult DetailsRooms(decimal id)
        {
            var tRate = _context.TRate
                .Include(x => x.User)
                .Where(y=>y.RoomId == id)
                .ToList();

            return View(tRate);
        }

        public IActionResult RemoveRoom(decimal id)
        {
            List<TRoom> roomList = JsonConvert.DeserializeObject<List<TRoom>>(HttpContext.Session.GetString("rooms"));
            TRoom removeRoom = roomList.FirstOrDefault(x => x.RoomId == id);
            if (removeRoom != null) roomList.Remove(removeRoom);

            HttpContext.Session.SetString("rooms", JsonConvert.SerializeObject(roomList));

            return RedirectToAction("Rooms", new { id = -1 });
        }
        public IActionResult Reservation()
        {
            if (HttpContext.Session.GetInt32("pending") != 1)
            {
                return RedirectToAction(nameof(Index));
            }

            var listRoom = JsonConvert.DeserializeObject<List<TRoom>>(HttpContext.Session.GetString("rooms"));
            ViewBag.dateFrom = DateTime.Parse(HttpContext.Session.GetString("resFrom")).ToString("yyyy - MMMM - dd");
            ViewBag.dateTo = DateTime.Parse(HttpContext.Session.GetString("resTo")).ToString("yyyy - MMMM - dd");

            if (listRoom.Count()<1)
            {
                return RedirectToAction(nameof(Category));
            }

            var rooms = _context.TRoom
                .Include(x => x.Cat)
                .Where(y => listRoom.Select(z => z.RoomId).Contains(y.RoomId)).ToList();

            return View(rooms);
        }

        public async System.Threading.Tasks.Task<IActionResult> MakeReservation()
        {
            if (HttpContext.Session.GetInt32("pending") != 1)
            {
                return RedirectToAction(nameof(Index));
            }

            var dateFrom = DateTime.Parse(HttpContext.Session.GetString("resFrom"));
            var dateTo = DateTime.Parse(HttpContext.Session.GetString("resTo"));
            var uid = HttpContext.Session.GetInt32("UID");

            var tRes = new TReservation { ResFrom = dateFrom, ResTo = dateTo, UserId = uid };

            _context.Add(tRes);
            if (await _context.SaveChangesAsync() > 0)
            {
                int resId = _context.TReservation.Max(x => x.ResId);
                var listRoom = JsonConvert.DeserializeObject<List<TRoom>>(HttpContext.Session.GetString("rooms"));
                var resRoom = new List<TReservationRoom>();

                foreach (var item in listRoom)
                {
                    var temp = new TReservationRoom { ResId = resId, RoomId = decimal.ToInt16(item.RoomId) };
                    resRoom.Add(temp);
                }

                _context.TReservationRoom.AddRange(resRoom);
                if (await _context.SaveChangesAsync() > 0)
                {
                    HttpContext.Session.SetInt32("pending", 0);
                    return RedirectToAction(nameof(Complete));
                }
                
            }

            TempData["Error"] = "Unable to create the reservation. Please try again";
            return RedirectToAction(nameof(Reservation));
        }

        public IActionResult Complete()
        {
            return View();
        }

        public IActionResult DetailCategory(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tCategory = _context.TCategory
                .Where(cat => cat.CatId == id)
                .Include(ic => ic.TImg)
                .FirstOrDefault();

            if (tCategory == null)
            {
                return NotFound();
            }

            return View(tCategory);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ManageReservation()
        {
            var uid = HttpContext.Session.GetInt32("UID");
            var tRes = _context.TReservation
                .Where(x => x.UserId == uid).ToList();

            return View(tRes);
        }

        public IActionResult DetailReservation()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            if (HttpContext.Session.GetInt32("UID") != -1 && HttpContext.Session.GetInt32("UID") != null)
            {
                TempData["User"] = true;
            }
            else
            {
                TempData["User"] = false;
            }
        }
    }
}
