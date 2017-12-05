using System.ComponentModel.DataAnnotations;

namespace Voidwell.UserManagement.Models
{
    public class EmailAddressRequest
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}
