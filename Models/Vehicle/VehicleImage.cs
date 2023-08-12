using System.ComponentModel.DataAnnotations.Schema;

namespace VeekelApi.Models.Vehicle
{
    public class VehicleImage
    {
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int AdID { get; set; }
        public virtual VehicleListing VehicleListing { get; set; }
        public string URI { get; set; }
    }
}
