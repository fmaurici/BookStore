using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD3.Models.Account;
using Entities;
using IBusiness.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD3.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public IActionResult LoginView(string returnUrl = "")
        {
            var model = new LogInViewModel { ReturnUrl = returnUrl };
            return View("Login", model);
        }

        public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager.CreateUser(model);
                if (result.Succeeded)
                {
                    return BuildToken(model);
                }
                else
                {
                    string errorMessages = _accountManager.GetAuthenticationErrors(result);
                    return BadRequest(errorMessages);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogInViewModel logInViewModel)
        {
            if (ModelState.IsValid)
            {
                var userInfo = new UserInfo() { Email = logInViewModel.Email, Password = logInViewModel.Password };
                var result = await _accountManager.LogIn(userInfo);
                if (result.Succeeded)
                {
                    var token = _accountManager.BuildToken(userInfo);
                    //CookieOptions option = new CookieOptions();
                    //Response.Cookies.Append("token","Bearer " + token.Token, option);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _accountManager.LogOut();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult BuildToken(UserInfo userInfo)
        {
            var tokenObject = _accountManager.BuildToken(userInfo);

            return Ok(new
            {
                token = tokenObject.Token,
                expiration = tokenObject.Expiration
            });
        }

    }
}