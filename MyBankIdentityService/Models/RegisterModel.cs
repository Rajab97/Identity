using MyBankIdentityService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models
{
    public class RegisterModel
    {
        public string Identity { get; set; }

        public int RoleId { get; set; }
    }
}
