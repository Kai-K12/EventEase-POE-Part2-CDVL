using POE_PART1_V5.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POE_PART1_V5.Models
{
    public class Venue
    {
        [Key]
        public int VenueID { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        public string VenueName { get; set; }

        [Required(ErrorMessage = "Venue location is required")]
        public string VenueLocation { get; set; }

        [Required(ErrorMessage = "Venue capacity is required")]
        [Range(1, 10000, ErrorMessage = "Capacity must be a positive number")]
        public int VenueCapacity { get; set; }

        public string? VenueImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}