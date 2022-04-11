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
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public double Price { get; set; }
        private Mechanic Mechanics { get; set; }
        public virtual ICollection<Car> Cars { get; set; }

    }
}