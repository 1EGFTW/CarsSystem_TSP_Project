using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarSystem_TSP_Project.Models
{
    public class Car
    {

        [Key]
        public int CarId { get; set; }


        [Required(ErrorMessage ="This field is required!")]
        [MinLength(2,ErrorMessage="Minimum length is 2 characters!")]
        public string Manufacturer { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        [MinLength(3, ErrorMessage = "Minimum length is 3 characters!")]
        public string Model { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        public string Engine { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        public string Transmission { get; set; }


        [Required(ErrorMessage = "This field is required!")]

        [DisplayName("Drive Type")]
        public string DriveType { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        [MinLength(17, ErrorMessage = "Length must be 17 characters!")]
        [MaxLength(17, ErrorMessage = "Length must be 17 characters!")]
        public string Vin { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        public double Price { get; set; }

        [DisplayName("Date of first registration")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]

        public DateTime DateOfFirstReg { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        public int Mileage { get; set; }


        [DisplayName("Owner")]
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }


        [DisplayName("Payment")]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        [DisplayName("Discount in %")]
        public double Discount { get; set; }


        [Required(ErrorMessage = "This field is required!")]

        [DisplayName("Vehicle Type")]
        public string VehicleType { get; set; }


        [DisplayName("Service")]
        public int ServiceId { get; set; }
        public Service Services { get; set; }

    }
}