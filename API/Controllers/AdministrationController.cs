using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using IBusiness.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;

        public AdministrationController(IAccountManager accountManager, IMapper mapper)
        {
            _accountManager = accountManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleInfo roleInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountManager.CreateRole(roleInfo);

            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]UserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountManager.UpdateUser(userInfo);

            if (!result.Succeeded)
            {
                string errorMessages = _accountManager.GetAuthenticationErrors(result);
                ModelState.AddModelError(string.Empty, errorMessages);
                return BadRequest(ModelState);
            }

            return Ok(result);

        }
    }
}