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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            var roleInfo = _mapper.Map<RoleInfo>(model);
            await _accountManager.CreateRole(roleInfo);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddUserToRole(AdministrationViewModel model)
        {
            await _accountManager.AddUserToRole(model.SelectedUser.ToString(), model.SelectedRole.ToString());

            return RedirectToAction("Index");
        }
    }
}