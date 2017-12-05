using System.ComponentModel.DataAnnotations;

namespace Voidwell.UserManagement.Models
{
    public class ResetPasswordStart
    {
        [Required]
        public string Email { get; set; }
    }
}
