using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using funduq.Models;

namespace funduq.Controllers
{
    public class SpecialEventController : Controller
    {
        private readonly HotelReservationContext _context;

        public SpecialEventController(HotelReservationContext context)
        {
            _context = context;
        }

        // GET: SpecialEvent
        public async Task<IActionResult> Index()
        {
            var hotelReservationContext = _context.SpecialEvents.Include(s => s.Hotel);
            return View(await hotelReservationContext.ToListAsync());
        }

        // GET: SpecialEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SpecialEvents == null)
            {
                return NotFound();
            }

            var specialEvent = await _context.SpecialEvents
                .Include(s => s.Hotel)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (specialEvent == null)
            {
                return NotFound();
            }

            return View(specialEvent);
        }

        // GET: SpecialEvent/Create
        public IActionResult Create()
        {
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId");
            return View();
        }

        // POST: SpecialEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,HotelId,EventDescription,EventPicture,TicketPrice,MaximumTickets,AvailableFrom,AvailableTo")] SpecialEvent specialEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", specialEvent.HotelId);
            return View(specialEvent);
        }

        // GET: SpecialEvent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SpecialEvents == null)
            {
                return NotFound();
            }

            var specialEvent = await _context.SpecialEvents.FindAsync(id);
            if (specialEvent == null)
            {
                return NotFound();
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", specialEvent.HotelId);
            return View(specialEvent);
        }

        // POST: SpecialEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,HotelId,EventDescription,EventPicture,TicketPrice,MaximumTickets,AvailableFrom,AvailableTo")] SpecialEvent specialEvent)
        {
            if (id != specialEvent.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialEventExists(specialEvent.EventId))
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
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", specialEvent.HotelId);
            return View(specialEvent);
        }

        // GET: SpecialEvent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SpecialEvents == null)
            {
                return NotFound();
            }

            var specialEvent = await _context.SpecialEvents
                .Include(s => s.Hotel)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (specialEvent == null)
            {
                return NotFound();
            }

            return View(specialEvent);
        }

        // POST: SpecialEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SpecialEvents == null)
            {
                return Problem("Entity set 'HotelReservationContext.SpecialEvents'  is null.");
            }
            var specialEvent = await _context.SpecialEvents.FindAsync(id);
            if (specialEvent != null)
            {
                _context.SpecialEvents.Remove(specialEvent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialEventExists(int id)
        {
          return (_context.SpecialEvents?.Any(e => e.EventId == id)).GetValueOrDefault();
        }
    }
}
