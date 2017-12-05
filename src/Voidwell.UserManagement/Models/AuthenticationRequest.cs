using System.ComponentModel.DataAnnotations;

namespace Voidwell.UserManagement.Models
{
    public class AuthenticationRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
