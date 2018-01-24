namespace Voidwell.UserManagement.Models
{
    public class UserLockRequest
    {
        public bool? IsPermanant { get; set; }
        public int? LockLength { get; set; }
    }
}
