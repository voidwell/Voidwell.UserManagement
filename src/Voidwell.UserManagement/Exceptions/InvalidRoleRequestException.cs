using System;

namespace Voidwell.UserManagement.Exceptions
{
    public class InvalidRoleRequestException : Exception
    {
        public InvalidRoleRequestException()
        { }

        public InvalidRoleRequestException(string message)
            :base(message)
        { }
    }
}
