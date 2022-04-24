using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBankIdentityService.Helpers.Attributes;
using MyBankIdentityService.Models;
using MyBankIdentityService.Models.ServiceModels;
using MyBankIdentityService.Services.Interfaces;
using Serilog;

namespace MyBankIdentityService.Controllers
{
    [Route("mb/api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IActiveDirectoryService _activeDirectory;
        private readonly IAccountService _accountService;


        public AccountController(IActiveDirectoryService activeDirectory, IAccountService accountService)
        {
            _activeDirectory = activeDirectory;
            _accountService = accountService;
        }

        [HttpPost("update/branches")]
        [ProducesResponseType(typeof(BaseServiceResponse), 200)]
        [ProducesResponseType(typeof(BaseServiceResponse), 500)]
        public async Task<IActionResult> UpdateUsersBranchInformation()
        {
            BaseServiceResponse serviceResponse = await _accountService.UpdateUsersBranchInformation();
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(BaseServiceResponse), 200)]
        [ProducesResponseType(typeof(BaseServiceResponse), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            BaseServiceResponse serviceResponse = await _accountService.Register(registerModel);
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest request)
        { 
            var response = await _accountService.Login(request);
            return StatusCode(response.StatusCode, response.Data);
        }

        [HttpGet("logout/{id}")]
        public async Task<IActionResult> Logout([FromQuery] int id)
        {
            var response = await _accountService.Logout(id);
            return StatusCode(response.StatusCode, response.Data);
        }


        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel refreshToken)
        {
            var response = await _accountService.RefreshToken(refreshToken.Token);
            return StatusCode(response.StatusCode, response.Data);
        }


        [HttpPost("editRole")]
        public async Task<IActionResult> EditRole([FromBody] UserRoleModel request)
        {
            var response = await _accountService.EditRole(request);
            return StatusCode(response.StatusCode, response.Data);
        }

        [HttpGet("users")]
        public async Task<IActionResult> Users([FromQuery] UsersQueryParam usersQueryParam)
        {
            var response = await _accountService.Users(usersQueryParam);
            return StatusCode(response.StatusCode, response.Data);
        }

        [HttpPut("status")]
        public async Task<IActionResult> ChangeStatusOfUser ([FromBody] UserStatusModel model)
        {
            var response = await _accountService.ChangeActivation(model);
            return StatusCode(response.StatusCode, response.Data);
        }

        [HttpPut("editBranch")]
        public async Task<IActionResult> EditBranch([FromBody] UserBranchModel request)
        {
            var response = await _accountService.EditBranch(request);
            return StatusCode(response.StatusCode, response.Data);
        }

        //[HttpGet("users")]
        //public async Task<IActionResult> Users()
        //{
        //    var response = await _accountService.Users();
        //    return StatusCode(response.StatusCode, response.Data);
        //}


        //[HttpGet("usersByQuery")]
        //public async Task<IActionResult> Users([FromQuery] UsersQueryParam usersQueryParam)
        //{
        //    var response = await _accountService.Users(usersQueryParam);
        //    return StatusCode(response.StatusCode, response.Data);
        //}



    }
}
