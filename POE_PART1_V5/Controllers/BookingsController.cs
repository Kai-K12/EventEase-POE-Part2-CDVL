using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POE_PART1_V5.Data;
using POE_PART1_V5.Models;

namespace POE_PART1_V5.Controllers
{
    public class BookingsController : Controller
    {
        private readonly POE_PART1_V5Context _context;

        public BookingsController(POE_PART1_V5Context context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string? searchString)
        {
            var bookings = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                    b.BookingID.ToString().Contains(searchString) ||
                    (b.Event != null && b.Event.EventName!.Contains(searchString)));
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

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventName");
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName");
            return View();
        }


        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingID,BookingDate,EventID,VenueID,BookingEndDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Double booking validation
                bool isOverlapping = _context.Booking.Any(b =>
                    b.VenueID == booking.VenueID &&
                    ((booking.BookingDate >= b.BookingDate && booking.BookingDate < b.BookingEndDate) ||
                     (booking.BookingEndDate > b.BookingDate && booking.BookingEndDate <= b.BookingEndDate) ||
                     (booking.BookingDate <= b.BookingDate && booking.BookingEndDate >= b.BookingEndDate))
                );

                if (isOverlapping)
                {
                    ModelState.AddModelError("", "The selected venue is already booked during the selected date range.");
                }
                else
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingID,BookingDate,EventID,VenueID,BookingEndDate")] Booking booking)
        {
            if (id != booking.BookingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Double booking check (excluding current record)
                bool isOverlapping = _context.Booking.Any(b =>
                    b.BookingID != booking.BookingID &&
                    b.VenueID == booking.VenueID &&
                    ((booking.BookingDate >= b.BookingDate && booking.BookingDate < b.BookingEndDate) ||
                     (booking.BookingEndDate > b.BookingDate && booking.BookingEndDate <= b.BookingEndDate) ||
                     (booking.BookingDate <= b.BookingDate && booking.BookingEndDate >= b.BookingEndDate))
                );

                if (isOverlapping)
                {
                    ModelState.AddModelError("", "The selected venue is already booked during the selected date range.");
                }
                else
                {
                    try
                    {
                        _context.Update(booking);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BookingExists(booking.BookingID))
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
            }

            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventName", booking.EventID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", booking.VenueID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);
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
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingID == id);
        }

        // GET: Bookings/Filter
        public IActionResult Filter(string? searchTerm)
        {
            var bookings = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                bookings = bookings.Where(b =>
                    b.BookingID.ToString().Contains(searchTerm) ||
                    (b.Event != null && b.Event.EventName.Contains(searchTerm)));
            }

            return View(bookings.ToList());
        }
    }
}
