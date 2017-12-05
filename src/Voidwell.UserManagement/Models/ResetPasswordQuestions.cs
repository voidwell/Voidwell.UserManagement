using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Voidwell.UserManagement.Models
{
    public class ResetPasswordQuestions
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        public IEnumerable<SecurityQuestion> Questions { get; set; }
    }
}
