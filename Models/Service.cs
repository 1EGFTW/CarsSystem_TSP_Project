using System.ComponentModel.DataAnnotations;

namespace CarSystem_TSP_Project.Models
{
    public class Service
    {
        public Service()
        {

            this.Cars = new HashSet<Car>();
        }
        [Key]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(3, ErrorMessage = "Minimum length is 3 characters!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(3, ErrorMessage = "Minimum length is 3 characters!")]
        public string Type { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public double Price { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        private Mechanic Mechanics { get; set; }
        public virtual ICollection<Car> Cars { get; set; }

    }
}