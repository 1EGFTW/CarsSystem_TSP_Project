using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarSystem_TSP_Project.Models
{
    public class Owner
    {
        [Key]
        public int OwnerId { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(2, ErrorMessage = "Minimum length is 2 characters!")]
        public string Name { get; set; }

        [DisplayName("Cars Bought")]
        [Required(ErrorMessage = "This field is required!")]

        public int CarsBought { get; set; } = 0;

        public virtual ICollection<Car> Car { get; set; }
    }
}