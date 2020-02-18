using AutoMapper;
using Entities;
using IBusiness.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Account
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountManager(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<SignInResult> LogIn(UserInfo userInfo)
        {
            //isPersistent helps to store the cookie after the session ends (should only when you press remember Me in login)
            return await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: userInfo.RememberMe, lockoutOnFailure: false);
        }

        public async Task<SignInResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return SignInResult.Success;
        }

        public async Task<IdentityResult> CreateUser(UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            return await _userManager.CreateAsync(user, model.Password);
        }

        public AuthenticationToken BuildToken(UserInfo userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                //new Claim("miValor", "Lo que yo quiera"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "yourdomain.com",
               audience: "yourdomain.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);

            var tokenObject = new AuthenticationToken() { Token = writtenToken, Expiration = expiration };

            return tokenObject;
        }

        public string GetAuthenticationErrors(IdentityResult result)
        {
            string errorMessages = string.Empty;
            foreach (IdentityError error in result.Errors)
            {
                errorMessages = errorMessages + ", " + error.Description;
            }

            return "Username or password invalid " + errorMessages;
        }

        public async Task<IList<UserInfo>> GetAllUsers()
        {
            IList<ApplicationUser> users = await _userManager.Users.ToListAsync();

            var usersInfo = users.Select(x => _mapper.Map<UserInfo>(x)).ToList();

            return usersInfo;
        }

        public async Task<IList<UserInfo>> GetAllUsersWithRoles()
        {
            var usersInfo = new List<UserInfo>();
            IList<ApplicationUser> users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInfo = _mapper.Map<UserInfo>(user);
                var roles = await GetRolesByUser(user);
                var rolesInfo = roles.Select(x => _mapper.Map<RoleInfo>(x)).ToList();
                userInfo.Roles = rolesInfo;
                usersInfo.Add(userInfo);
            }

            return usersInfo;
        }


        public async Task<IList<RoleInfo>> GetRolesByUser(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var rolesInfo = roles.Select(x => new RoleInfo() { Name = x }).ToList();
            return rolesInfo;
        }

        public async Task<UserInfo> GetUserById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userInfo = _mapper.Map<UserInfo>(user);
            return userInfo;
        }

        public async Task<UserInfo> GetUserWithRolesById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roles = await GetRolesByUser(user);
            var userInfo = _mapper.Map<UserInfo>(user);
            userInfo.Roles = roles;
            return userInfo;
        }

        public async Task<RoleInfo> GetRoleById(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            var roleInfo = _mapper.Map<RoleInfo>(role);
            return roleInfo;
        }

        public async Task<IdentityResult> CreateRole(RoleInfo roleInfo)
        {
            ApplicationRole role = _mapper.Map<ApplicationRole>(roleInfo);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRole(RoleInfo roleInfo)
        {
            ApplicationRole role = _mapper.Map<ApplicationRole>(roleInfo);
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> UpdateUser(UserInfo userInfo)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userInfo.Id.ToString());
            userToUpdate.FirstName = userInfo.FirstName;
            userToUpdate.LastName = userInfo.LastName;

            await AddUserToRole(userInfo.Id, userInfo.Roles.Select(x => x.Name).ToList());

            return await _userManager.UpdateAsync(userToUpdate);
        }

        public async Task<IdentityResult> AddUserToRole(Guid userId, IList<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            foreach (var roleName in roleNames)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }

            return IdentityResult.Success;
        }
    }
}
