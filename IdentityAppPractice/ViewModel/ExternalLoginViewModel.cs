using System.ComponentModel.DataAnnotations;

namespace IdentityAppPractice.ViewModel
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
