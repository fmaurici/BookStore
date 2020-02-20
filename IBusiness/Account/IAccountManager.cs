using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IBusiness.Account
{
    public interface IAccountManager
    {
        Task<IdentityResult> CreateUserWithViewRole(UserInfo model);
        Task<IdentityResult> UpdateUser(UserInfo userInfo);
        AuthenticationToken BuildToken(UserInfo userInfo);
        string GetAuthenticationErrors(IdentityResult result);
        Task<SignInResult> LogIn(UserInfo userInfo);
        Task<SignInResult> LogOut();
        Task<UserInfo> GetUserById(Guid userId);
        Task<UserInfo> GetUserWithRolesById(Guid userId);
        Task<RoleInfo> GetRoleById(Guid roleId);
        Task<IdentityResult> CreateRole(RoleInfo role);
        Task<IList<UserInfo>> GetAllUsers();
        Task<IList<RoleInfo>> GetAllRoles();
        Task<IList<UserInfo>> GetAllUsersWithRoles();
        Task<IList<RoleInfo>> GetRolesByUser(ApplicationUser user);
    }
}
