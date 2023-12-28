using System.ComponentModel.DataAnnotations;

namespace SiteASPCOm.Models.DTO
{
    public class ChangePasswordModel
    {
        [Required]
        public string ? CurrentPassword { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{10,}$",ErrorMessage ="Minimum length 10 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
        public string? NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ? PasswordConfirm { get; set; }
        
    }
}
