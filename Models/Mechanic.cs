using System.ComponentModel.DataAnnotations;

namespace CarSystem_TSP_Project.Models
{
    public class Mechanic
    {
        public Mechanic()
        {
            this.Services = new HashSet<Service>();
        }
        [Key]
        public int MechanicId { get; set; }
        [Required]
        public string Name { get; set; }


        public virtual ICollection<Service> Services { get; set; }

    }
}