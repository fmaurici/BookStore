﻿using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IBusiness.Account
{
    public interface IAccountManager
    {
        Task<IdentityResult> CreateUser(UserInfo model);
        AuthenticationToken BuildToken(UserInfo userInfo);
        string GetAuthenticationErrors(IdentityResult result);
        Task<SignInResult> LogIn(UserInfo userInfo);
        Task<SignInResult> LogOut();
        Task<ApplicationUser> GetUserById(Guid selectedUser);
        Task<ApplicationRole> GetRoleById(Guid selectedRole);
        Task<IdentityResult> CreateRole(RoleInfo role);
        Task<IdentityResult> AddUserToRole(string userId, string roleId);
    }
}
