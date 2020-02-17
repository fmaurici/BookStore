using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD3.Models.Account
{
    public class AdministrationViewModel
    {
        public List<RoleViewModel> Roles { get; set; }
        public List<UserViewModel> Users { get; set; }

        public Guid SelectedUser { get; set; }
        public Guid SelectedRole { get; set; }
    }
}
