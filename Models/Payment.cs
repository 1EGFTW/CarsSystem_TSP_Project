
using System.ComponentModel.DataAnnotations;

namespace CarSystem_TSP_Project.Models
{
    public class Payment
    {

        [Key]
        public int PaymentId { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(3, ErrorMessage = "Minimum length is 3 characters!")]
        public string Type { get; set; }
        public virtual ICollection<Car> Car { get; set; }
    }
}