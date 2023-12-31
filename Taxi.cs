using System.ComponentModel.DataAnnotations;

namespace AmneetTest2.Models
{
    public class Taxi
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Please enter a valid name with alphabetic characters only.")]
        public string DriverName { get; set; }
        [Required]
        public string CarModel { get; set; }

        [Required]
        public string LicensePlate { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;

    }
}
