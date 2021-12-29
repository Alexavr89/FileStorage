using System.ComponentModel.DataAnnotations;

namespace File_Storage.Models
{
    public class LogonModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
