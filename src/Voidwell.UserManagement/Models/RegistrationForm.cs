using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Voidwell.UserManagement.Models
{
    public class RegistrationForm
    {
        [Required(ErrorMessage = "Must include a username")]
        [MinLength(3, ErrorMessage = "Username must be 3 or more characters long")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username contains invalid characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Must include a password")]
        [MinLength(6, ErrorMessage = "Password must be 6 or more characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Must include an email address")]
        [EmailAddress(ErrorMessage = "Must use valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must include at least 3 security questions")]
        [MinLength(3, ErrorMessage = "Must include at least 3 security questions")]
        public IEnumerable<SecurityQuestion> SecurityQuestions { get; set; }
    }
}
