using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Services.Interfaces;

namespace MyBankIdentityService.Controllers
{
    [Route("mb/api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        //private readonly IGenericService<Role> _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result.Data);
        }

        // GET: api/Role/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            //var result = await _service.GetByIdAsync(id);
            var result = await _service.GetRoleAsync(id);
            return StatusCode(result.StatusCode, result.Data);
        }

        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Role value)
        {
            var result = await _service.AddAsync(value);
            return StatusCode(result.StatusCode, result.Data);
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Role value)
        {
            if (id != value.Id)
            {
                return BadRequest("Request model id is not equal to the query id");
            }
            var result = await _service.UpdateAsync(value);
            return StatusCode(result.StatusCode, result.Data);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
