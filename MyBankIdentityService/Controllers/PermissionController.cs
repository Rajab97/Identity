using Microsoft.AspNetCore.Mvc;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Services.Interfaces;
using System.Threading.Tasks;

namespace MyBankIdentityService.Controllers
{
    [Route("mb/api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
     
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // GET: api/Permission
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? moduleId)
        {
            var result = await _permissionService.GetAllAsync(moduleId);
            return StatusCode(result.StatusCode, result.Data);
        }

        // GET: api/Permission/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _permissionService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result.Data);
        }

        // POST: api/Permission
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Permission value)
        {
            var result = await _permissionService.AddAsync(value);
            return StatusCode(result.StatusCode, result.Data);
        }

        // PUT: api/Permission/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Permission value)
        {
            if (id != value.Id)
            {
                return BadRequest("Request model id is not equal to the query id");
            }
            var result = await _permissionService.UpdateAsync(value);
            return StatusCode(result.StatusCode, result.Data);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _permissionService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
