using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; } = null!;
    }
}
