using Microsoft.AspNetCore.Mvc;
using MyBankIdentityService.Models.DTO;
using MyBankIdentityService.Services.Interfaces;
using System.Threading.Tasks;

namespace MyBankIdentityService.Controllers
{
    [Route("mb/api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService branchService;

        public BranchController(IBranchService branchService)
        {
            this.branchService = branchService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] KafkaBranchMessageValueModel request)
        {
            var response = await branchService.SumbitBranchModel(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await branchService.GetBranches();
            return StatusCode(response.StatusCode, response);
        }
    }
}
