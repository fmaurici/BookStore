using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRUD3.Models.Account;
using Entities;
using IBusiness.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRUD3.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;

        public AdministrationController(IAccountManager accountManager, IMapper mapper)
        {
            _accountManager = accountManager;
            _mapper = mapper;
        }

        public IActionResult AddRole()
        {
            return View();
        }

        public async Task<IActionResult> EditUser(Guid id)
        {
            var user = await _accountManager.GetUserWithRolesById(id);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            var allRoles = await _accountManager.GetAllRoles();
            userViewModel.Roles = allRoles.Select(x => _mapper.Map<RoleViewModel>(x)).ToList(); 
            userViewModel.SelectedRoles = user.Roles.Select(x => x.Name).ToArray();

            return View(userViewModel);
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new AdministrationViewModel() { Users = new List<UserViewModel>()};
            var users = await _accountManager.GetAllUsersWithRoles();

            viewModel.Users = users.Select(x => _mapper.Map<UserViewModel>(x)).ToList();
            
            return View(viewModel);
        }

        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            var roleInfo = _mapper.Map<RoleInfo>(model);
            await _accountManager.CreateRole(roleInfo);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var userInfo = _mapper.Map<UserInfo>(model);
            var result = await _accountManager.UpdateUser(userInfo);

            if (!result.Succeeded)
            {
                string errorMessages = _accountManager.GetAuthenticationErrors(result);
                ModelState.AddModelError(string.Empty, errorMessages);
                return View("Index", model);
            }
            
            return RedirectToAction("Index");

        }
    }
}