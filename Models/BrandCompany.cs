using System.ComponentModel.DataAnnotations.Schema;

namespace VeekelApi.Models
{
    public class BrandCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public Country country { get; set; }

    }
}
