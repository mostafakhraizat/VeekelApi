
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VeekelApi.Models;

namespace Common.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int? LanguageId { get; set; }
        //public virtual Language Language { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(12)]
        public string NickName { get; set; }

        public int User_type { get; set; }

        public int Status { get; set; }
        public string TFAActivationCode { get; set; }
        public bool Email2FAEnabled { get; set; }

        public int TutorialFlag { get; set; }
        public int OneClickTradingFlag { get; set; }
        //[NotMapped]
        //public ICollection<Models.MamAccount.MamAccount> MamAccount { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
