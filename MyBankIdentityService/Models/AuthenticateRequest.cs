using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models
{
    public class AuthenticateRequest
    {
        public string Identity { get; set; }
        public string Password { get; set; }
    }
}
