
using Common.Data;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using VeekelApi.Models.Authentication;
using static SocialApi.Models.Authentication.JwtAuthenticationManager;

namespace SocialApi.Models.Authentication
{
    public interface IJwtAuthenticationManager
    {
        Task<object> Authenticate(AuthenticateRequest model);
        Task<RegisterReturn> RegisterAsync(RegisterModel user);
         public string GenerateJwtToken(ApplicationUser user);

    }
}
