using System.ComponentModel.DataAnnotations;

namespace CarSystem_TSP_Project.Models
{
    public class Owner
    {
        [Key]
        public int OwnerId { get; set; }
        [Required]
        public string Name { get; set; }
        public int CarsBought { get; set; } = 0;

        public ICollection<Car> Car { get; set; }
    }
}