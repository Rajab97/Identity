using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models.ConfigModels
{
    public class ActiveDirectoryConfig
    {
        public string Host { get; set; }
        public string DistinguishedName { get; set; }
        public string Password { get; set; }
        public string OrganizationUnit { get; set; }
    }
}
