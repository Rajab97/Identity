using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyBankIdentity.Attributes;
using MyBankIdentity.Controllers;

namespace MyBankIdentityService.Controllers
{
    [Route("mb/api/[controller]")]
    [ApiController]
    public class DemoController : MyController
    {
        // GET: api/Demo
        [HttpGet]
        //[MyBankAuth("reada")]
        [MyBankAuth("read", "create")]
        public IEnumerable<string> Get()
        {
            
            return new string[] { CurrentUser.Fullname, "HrCode: " + CurrentUser.HrCode };
        }

       
    }
}
