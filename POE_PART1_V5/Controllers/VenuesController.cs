using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POE_PART1_V5.Data;
using POE_PART1_V5.Models;
using POE_PART1_V5.Services;

namespace POE_PART1_V5.Controllers
{
    public class VenuesController : Controller
    {
        private readonly POE_PART1_V5Context _context;
        private readonly BlobService _blobService;

        public VenuesController(POE_PART1_V5Context context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venue.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueID,VenueName,VenueLocation,VenueCapacity,VenueImageUrl")] Venue venue, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Upload image to Azure Blob Storage if provided
                if (imageFile != null)
                {
                    venue.VenueImageUrl = await _blobService.UploadImageAsync(imageFile);
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueID,VenueName,VenueLocation,VenueCapacity,VenueImageUrl, ImageFile")] Venue venue)
        {
            if (id != venue.VenueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingVenue = await _context.Venue.AsNoTracking().FirstOrDefaultAsync(v => v.VenueID == id);

                    if (venue.ImageFile != null)
                    {
                        if (!string.IsNullOrEmpty(existingVenue?.VenueImageUrl) &&
                            existingVenue.VenueImageUrl.Contains("blob.core.windows.net"))
                        {
                            await _blobService.DeleteImageAsync(existingVenue.VenueImageUrl);
                        }

                        venue.VenueImageUrl = await _blobService.UploadImageAsync(venue.ImageFile);
                    }
                    else
                    {
                        if (existingVenue != null)
                        {
                            venue.VenueImageUrl = existingVenue.VenueImageUrl;
                        }
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueID))
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
            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venue.FindAsync(id);

            if (venue == null)
            {
                return NotFound();
            }

            // Check if the venue is linked to any bookings
            bool hasBookings = await _context.Booking.AnyAsync(b => b.VenueID == id);

            if (hasBookings)
            {
                ModelState.AddModelError(string.Empty, "This venue is associated with bookings and cannot be deleted.");
                return View(venue); // return to Delete.cshtml with error
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueID == id);
        }
    }
}
