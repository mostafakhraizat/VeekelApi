using System.ComponentModel.DataAnnotations;

namespace SocialApi.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$", ErrorMessage = "Email Format is wrong")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
     

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
      


        [Required]
        [Display(Name = "Country")]
        public int Country { get; set; }

        public int UserType { get; set; }
        public int Status { get; set; }
    }
}