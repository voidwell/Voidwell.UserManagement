using System;
using System.Collections.Generic;

namespace Voidwell.UserManagement.Models
{
    public class BatchUserRequest
    {
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
