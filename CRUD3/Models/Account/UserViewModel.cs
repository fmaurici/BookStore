using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD3.Models.Account
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string SecurityStamp { get; set; }
        public string UserName { get; set; }
        public string Name { get { return FirstName + " " + LastName; } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public List<SelectListItem> RoleList { get { return RolesToSelectList(); } }
        public List<SelectListItem> RolesToSelectList()
        {
            return Roles.Select(role => new SelectListItem {Value = role.Name, Text = role.Name}).ToList();
        }

    }
}
