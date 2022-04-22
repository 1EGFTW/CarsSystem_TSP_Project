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
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(2, ErrorMessage = "Minimum length is 2 characters!")]
        public string Name { get; set; }


        public virtual ICollection<Service> Services { get; set; }

    }
}