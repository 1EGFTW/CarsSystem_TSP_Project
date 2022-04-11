using System.ComponentModel.DataAnnotations;

namespace CarSystem_TSP_Project.Models
{
    public class Car
    {

        [Key]
        public int CarId { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Engine { get; set; }
        [Required]
        public string Transmission { get; set; }
        [Required]
        public string DriveType { get; set; }
        [Required]
        public string Vin { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime DateOfFirstReg { get; set; }
        [Required]
        public int Mileage { get; set; }

        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public double Discount { get; set; }
        [Required]
        public string VehicleType { get; set; }
        public int ServiceId { get; set; }
        public Service Services { get; set; }

    }
}