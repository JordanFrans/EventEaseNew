using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Bookings (WITH SEARCH)
        public async Task<IActionResult> Index(string search)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                bookings = bookings.Where(b =>
                    b.Event.Name.Contains(search) ||
                    b.Venue.Name.Contains(search) ||
                    b.StartDate.ToString().Contains(search) ||
                    b.EndDate.ToString().Contains(search)
                );
            }

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name");
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name");
            return View();
        }

        // POST: Bookings/Create (WITH VALIDATION)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,EventId,StartDate,EndDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var exists = await _context.Bookings.AnyAsync(b =>
                    b.VenueId == booking.VenueId &&
                    booking.StartDate < b.EndDate &&
                    booking.EndDate > b.StartDate
                );

                if (exists)
                {
                    ModelState.AddModelError("", "This venue is already booked for the selected dates.");
                }
                else
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,VenueId,EventId,StartDate,EndDate")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Name", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}