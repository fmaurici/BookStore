using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class UserInfo : BaseEntity
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string SecurityStamp { get; set; }
        public IList<RoleInfo> Roles { get; set; }
}
}
