using System.ComponentModel.DataAnnotations;

namespace File_Storage.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
        public string Login { get; set; }
    }
}
