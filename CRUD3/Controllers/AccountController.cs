using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRUD3.Models.Account;
using Entities;
using IBusiness.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CRUD3.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;

        public AccountController(IAccountManager accountManager, IMapper mapper)
        {
            _accountManager = accountManager;
           _mapper = mapper;
        }

        public IActionResult Login(string returnUrl = "")
        {
            var model = new LogInViewModel { ReturnUrl = returnUrl };
            return View("Login", model);
        }

        public IActionResult Register(string returnUrl = "")
        {
            var model = new RegisterViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        public async Task<IActionResult> CreateUser(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", registerViewModel);
            }

            var userInfo = _mapper.Map<UserInfo>(registerViewModel);  
            var result = await _accountManager.CreateUserWithViewRole(userInfo);

            if (result.Succeeded)
            {
                var loginViewModel = _mapper.Map<LogInViewModel>(registerViewModel);
                return await SignIn(loginViewModel);
            }
            else
            {
                string errorMessages = _accountManager.GetAuthenticationErrors(result);
                ModelState.AddModelError(string.Empty, errorMessages);
                return View("Register", registerViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LogInViewModel logInViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("LogIn", logInViewModel);
            }

            var userInfo = _mapper.Map<UserInfo>(logInViewModel);
            var result = await _accountManager.LogIn(userInfo);
            
            if (result.Succeeded)
            {
                var returnUrl = logInViewModel.ReturnUrl ?? "/Home/Index";
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "The username or password do not match.");
                return View("LogIn", logInViewModel);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _accountManager.LogOut();
            return RedirectToAction("Index", "Home");
        }

    }
}