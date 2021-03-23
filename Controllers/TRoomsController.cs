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
    public class TRoomsController : Controller
    {
        private readonly RoomReservationContext _context;

        public TRoomsController(RoomReservationContext context)
        {
            _context = context;
        }

        // GET: TRooms
        public async Task<IActionResult> Index()
        {
            var roomReservationContext = _context.TRoom.Include(t => t.Cat);
            return View(await roomReservationContext.ToListAsync());
        }

        // GET: TRooms/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tRoom = await _context.TRoom
                .Include(t => t.Cat)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (tRoom == null)
            {
                return NotFound();
            }

            return View(tRoom);
        }

        // GET: TRooms/Create
        public IActionResult Create()
        {
            ViewData["CatId"] = new SelectList(_context.TCategory, "CatId", "CatId");
            return View();
        }

        // POST: TRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomFloor,RoomStatus,CatId")] TRoom tRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.TCategory, "CatId", "CatId", tRoom.CatId);
            return View(tRoom);
        }

        // GET: TRooms/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
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

        // POST: TRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("RoomId,RoomFloor,RoomStatus,CatId")] TRoom tRoom)
        {
            if (id != tRoom.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TRoomExists(tRoom.RoomId))
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
            ViewData["CatId"] = new SelectList(_context.TCategory, "CatId", "CatId", tRoom.CatId);
            return View(tRoom);
        }

        // GET: TRooms/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tRoom = await _context.TRoom
                .Include(t => t.Cat)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (tRoom == null)
            {
                return NotFound();
            }

            return View(tRoom);
        }

        // POST: TRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var tRoom = await _context.TRoom.FindAsync(id);
            _context.TRoom.Remove(tRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TRoomExists(decimal id)
        {
            return _context.TRoom.Any(e => e.RoomId == id);
        }
    }
}
