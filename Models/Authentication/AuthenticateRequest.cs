namespace VeekelApi.Models.Authentication
{
    public class AuthenticateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FCMToken { get; set; }
    }
}
