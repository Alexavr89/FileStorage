using System.ComponentModel.DataAnnotations;

namespace File_Storage.Models
{
    public class LogonModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
