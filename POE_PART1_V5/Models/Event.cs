using POE_PART1_V5.Models;
using System.ComponentModel.DataAnnotations;

namespace POE_PART1_V5.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Event start date is required")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Event end date is required")]
        public DateTime EventEndDate { get; set; }

        public string? EventDescription { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}