using Microsoft.AspNetCore.Authentication;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityAppPractice.ViewModel
{
    public class LoginViewModel
    {

        
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
