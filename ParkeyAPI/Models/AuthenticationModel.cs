using System.ComponentModel.DataAnnotations;

namespace ParkeyAPI.Models
{
    public class AuthenticationModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
