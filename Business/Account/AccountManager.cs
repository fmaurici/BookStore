using Entities;
using IBusiness.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IConfiguration _configuration;

        public AccountManager(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
            var result = await _userManager.CreateAsync(user, model.Password);

            return result;
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

        public async Task<ApplicationUser> GetUserById(Guid selectedUser)
        {
            return await _userManager.FindByIdAsync(selectedUser.ToString());
        }

        public async Task<ApplicationRole> GetRoleById(Guid selectedRole)
        {
            return await _roleManager.FindByIdAsync(selectedRole.ToString());
        }

        public async Task<IdentityResult> CreateRole(RoleInfo roleInfo)
        {
            var role = new ApplicationRole() { Name = roleInfo.Name };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> AddUserToRole(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            var roleName = await _roleManager.GetRoleNameAsync(role);

            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
