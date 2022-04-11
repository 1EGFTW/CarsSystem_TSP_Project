
using System.ComponentModel.DataAnnotations;

namespace CarSystem_TSP_Project.Models
{
    public class Payment
    {

        [Key]
        public int PaymentId { get; set; }
        [Required]
        public string Type { get; set; }
        public ICollection<Car> Car { get; set; }
    }
}