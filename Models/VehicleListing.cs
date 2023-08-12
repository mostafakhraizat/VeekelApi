using System.ComponentModel.DataAnnotations.Schema;

namespace VeekelApi.Models
{
    public class VehicleListing
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int Mileage { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public string FuelEfficiency { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string InteriorFeatures { get; set; }
        public string SafetyFeatures { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string VIN { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual User user { get; set; }

    }
}
