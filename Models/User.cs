using Microsoft.AspNetCore.Identity;

namespace VeekelApi.Models
{
    public class User: IdentityUser
    {
        public int CountryId{ get; set; }
        public int LanguageId{ get; set; }
    }
}
