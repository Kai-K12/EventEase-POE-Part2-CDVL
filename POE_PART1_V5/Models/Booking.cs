using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POE_PART1_V5.Models
{
    public class Booking : IValidatableObject
    {
        [Key]
        public int BookingID { get; set; }

        [Required(ErrorMessage = "Booking date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime? BookingDate { get; set; }

        [Required(ErrorMessage = "Booking end date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking End Date")]
        public DateTime? BookingEndDate { get; set; }

        [Required]
        public int EventID { get; set; }
        public Event? Event { get; set; }

        [Required]
        public int VenueID { get; set; }
        public Venue? Venue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BookingDate.HasValue && BookingEndDate.HasValue)
            {
                if (BookingEndDate < BookingDate)
                {
                    yield return new ValidationResult(
                        "Booking end date must be after the booking date.",
                        new[] { nameof(BookingEndDate) }
                    );
                }
            }
        }
    }
}
