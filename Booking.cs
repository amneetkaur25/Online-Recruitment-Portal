using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmneetTest2.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Please enter a valid name with alphabetic characters only.")]
        public string CustomerName { get; set; }
        [Required]
        public string PickupLocation { get; set; }

        [Required]
        public string DropOffLocation { get; set; }
        [Required]
        public DateTime BookingTime { get; set; }
        [Required]
        public int TaxiId { get; set; }
        //[Display(Name ="Taxi")]
        //public virtual int TaxiId { get; set; }

        //[ForeignKey("TaxiId")]
        //public virtual Taxi Taxi { get; set; }
    }
}
