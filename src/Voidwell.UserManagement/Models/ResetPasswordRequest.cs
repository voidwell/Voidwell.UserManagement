namespace Voidwell.UserManagement.Models
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
