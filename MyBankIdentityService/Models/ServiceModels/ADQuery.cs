using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models.ServiceModels
{
    public class ADQuery
    {
        public string Attribute { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public MatchType MatchType { get; set; }
    }
}
