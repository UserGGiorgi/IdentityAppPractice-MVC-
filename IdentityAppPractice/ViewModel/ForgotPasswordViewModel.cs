using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace IdentityAppPractice.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
