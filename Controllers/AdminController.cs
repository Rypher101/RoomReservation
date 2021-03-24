using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RoomReservation.Data;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoomReservationContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminController(RoomReservationContext context, IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
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
            var dict1 = new Dictionary<DateTime, int>();
            var dict2 = new Dictionary<string, int>();

            var c1 = _context.TReservation
                .Include(x => x.TReservationRoom)
                .ThenInclude(y => y.Room)
                .ToList();

            var c2 = _context.TCategory.ToDictionary(x=> x.CatId, x=>x.CatBed);

            foreach (var item in c1)
            {
                var key = new DateTime(item.ResDate.Year, item.ResDate.Month, 1);

                if (dict1.ContainsKey(key))
                {
                    int val = dict1[key];
                    foreach (var item2 in item.TReservationRoom)
                    {
                        val += c2[item2.Room.CatId];
                    }
                    dict1[key] = val;
                }
                else
                {
                    int val = 0;
                    foreach (var item2 in item.TReservationRoom)
                    {
                        val += c2[item2.Room.CatId];
                    }
                    dict1.Add(key, val);
                }
            }

            foreach (var item in c1)
            {
                foreach (var item2 in item.TReservationRoom)
                {
                    var key = item2.Room.CatId;

                    if (dict2.ContainsKey(key))
                    {
                        int val = dict2[key] + 1;
                        dict2[key] = val;
                    }
                    else
                    {
                        dict2.Add(key, 1);
                    }
                }
                
            }

            var ch1 = new List<C1>();
            var ch2 = new List<C2>();
            var total = dict2.Skip(1).Sum(x=>x.Value);

            foreach (var item in dict1)
            {
                var temp = new C1 { date = item.Key, val = item.Value };
                ch1.Add(temp);
            }

            foreach (var item in dict2)
            {
                var temp = new C2 { key = item.Key, val = item.Value*100/total };
                ch2.Add(temp);
            }

            ViewBag.C1 = ch1;
            ViewBag.C2 = ch2;

            return View();
        }

        #region Category
        public IActionResult ViewCategory()
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }

            SetDashboard(2);
            var tCat = _context.TCategory.ToList();
            return View(tCat);
        }

        public IActionResult NewCategory()
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
            SetDashboard(2);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewCategory([Bind("CatId,CatType,CatBed,CatDescription,CatPrice")] TCategory tCat)
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
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
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
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
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
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

        // GET: TCategories/Details/5
        public IActionResult DetailsCategory(string id)
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
            SetDashboard(2);
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

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormCollection collection)
        {
            var imgModel = new TImg();
            IFormFile files = HttpContext.Request.Form.Files.First();

            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(files.FileName);
            string extension = Path.GetExtension(files.FileName);
            imgModel.ImPath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imgModel.CatId = collection["catID"].ToString();
            string path = Path.Combine(wwwRootPath + "/Image/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await files.CopyToAsync(fileStream);
            }

            _context.Add(imgModel);
            await _context.SaveChangesAsync();

            return RedirectToAction("DetailsCategory", new { id = imgModel.CatId });
        }

        public async Task<IActionResult> DeleteImg(int id, string catId)
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
            var img = _context.TImg
                .FirstOrDefault(im => im.ImId == id);

            if (img != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string path = wwwRootPath + "\\Image\\" + img.ImPath;
                FileInfo fi = new FileInfo(path);

                if (fi.Exists)
                {
                    fi.Delete();
                    var tImg = await _context.TImg.FindAsync(id);
                    _context.TImg.Remove(tImg);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("DetailsCategory", new { id = catId });
        }
        #endregion

        #region Room
        public IActionResult ViewRoom()
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }

            SetDashboard(3);
            List<TRoom> tRoom = (from rm in _context.TRoom
                                 join rt in _context.TRate on rm.RoomId equals rt.RoomId into roomRate
                                 from rr in roomRate.DefaultIfEmpty()
                                 orderby rm.RoomId ascending
                                 select rm).ToList();

            List<TRoom> tRoomRes = (from rm in _context.TRoom
                                    join rr in _context.TReservationRoom on rm.RoomId equals rr.RoomId 
                                    join rs in _context.TReservation on rr.ResId equals rs.ResId 
                                    orderby rm.RoomId ascending
                                    select rm).ToList();

            foreach (var item in tRoomRes)
            {
                foreach (var item2 in tRoom)
                {
                    if (item2.RoomId == item.RoomId && item2.RoomStatus == 1)
                    {
                        item2.RoomStatus = 2;
                        break;
                    }
                    else if (item.RoomId > item2.RoomId)
                    {
                        break;
                    }
                }
            }

            return View(tRoom);
        }

        public IActionResult CreateRoom()
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
            ViewData["CatId"] = new SelectList(_context.TCategory, "CatId", "CatId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom([Bind("RoomId,RoomFloor,RoomStatus,CatId")] TRoom tRoom)
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                _context.Add(tRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.TCategory, "CatId", "CatId", tRoom.CatId);
            return RedirectToAction(nameof(ViewRoom));
        }

        public async Task<IActionResult> EditRoom(decimal? id)
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var tRoom = await _context.TRoom.FindAsync(id);
            if (tRoom == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = new SelectList(_context.TCategory, "CatId", "CatId", tRoom.CatId);
            return View(tRoom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoom(decimal id, [Bind("RoomId,RoomFloor,RoomStatus,CatId")] TRoom tRoom)
        {
            if (id != tRoom.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tRoom.RoomStatus = Convert.ToInt16(tRoom.RoomStatus);
                    _context.Update(tRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TRoom.Any(e => e.RoomId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewRoom));
            }
            ViewData["CatId"] = new SelectList(_context.TCategory, "CatId", "CatId", tRoom.CatId);
            return View(tRoom);
        }

        public async Task<IActionResult> DeleteRoom(decimal id)
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }

            var tRoom = new TRoom() { RoomId = id, RoomStatus = 0 };

            _context.Entry(tRoom).Property("RoomStatus").IsModified = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewRoom));
        }

        public async Task<IActionResult> ActiveRoom(decimal id)
        {
            if (HttpContext.Session.GetString("UType") != "A")
            {
                TempData["Error"] = "Insufficient login permission";
                return RedirectToAction("Index", "Login");
            }

            var tRoom = new TRoom() { RoomId = id, RoomStatus = 1 };

            _context.Entry(tRoom).Property("RoomStatus").IsModified = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewRoom));
        }
        #endregion
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
