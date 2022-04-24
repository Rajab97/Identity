using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Services.Interfaces;
using Serilog;

namespace MyBankIdentityService.Controllers
{
    [Route("mb/api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        //private readonly IGenericService<Module> _service;
        private readonly IModuleService _service;

        public ModuleController(IModuleService service)
        {
            _service = service;
        }

        // GET: api/Module
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Log.Information("Get modules called");
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result.Data);
        }

        // GET: api/Module/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result.Data);
        }

        // POST: api/Module
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Module value)
        {
            var result = await _service.AddAsync(value);
            return StatusCode(result.StatusCode, result.Data);
        }


        // PUT: api/Module/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Module value)
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
