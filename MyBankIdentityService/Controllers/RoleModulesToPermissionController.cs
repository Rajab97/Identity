using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBankIdentity.DTO;
using MyBankIdentityService.Services.Interfaces;

namespace MyBankIdentityService.Controllers
{
    [Route("mb/api/[controller]")]
    [ApiController]
    public class RoleModulesToPermissionController : ControllerBase
    {
        private readonly IRoleModulesToPermissionService _service;

        public RoleModulesToPermissionController(IRoleModulesToPermissionService service)
        {
            _service = service;
        }



        //// GET: api/RoleModulesToPermission
        //[HttpGet("{roleId}/{moduleId}")]
        //public async Task<IActionResult> Get( int roleId, int moduleId)
        //{
        //    var result = await _service.GetPermissionsAsync(roleId, moduleId);
        //    return StatusCode(result.StatusCode, result.Data);
        //}


        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetByRoleId(int roleId)
        {
            var result = await _service.GetPermissionsAsync(roleId);
            return StatusCode(result.StatusCode, result.Data);
        }


        //// GET: api/RoleModulesToPermission/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleModulesToPermissionDTO model)
        {
            var result = await _service.AddAsync(model);
            return StatusCode(result.StatusCode, result.Data);
        }

       

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode(result.StatusCode, result.Data);
        }

        // GET: api/RoleModulesToPermission
        [HttpDelete("{roleId}/{moduleId}")]
        public async Task<IActionResult> Delete(int roleId, int moduleId)
        {
            var result = await _service.DeleteRoleModuleAsync(roleId, moduleId);
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
