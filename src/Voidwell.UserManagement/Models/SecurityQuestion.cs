using System.ComponentModel.DataAnnotations;

namespace Voidwell.UserManagement.Models
{
    public class SecurityQuestion
    {
        [Required]
        public string Question { get; set; }

        [Required(ErrorMessage = "Must include an answer")]
        [MinLength(3, ErrorMessage = "Answer must have 3 or more characters")]
        public string Answer { get; set; }
    }
}
