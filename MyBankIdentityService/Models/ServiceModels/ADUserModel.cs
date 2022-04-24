using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models.ServiceModels
{
    public class ADUserModel
    {
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public string VoiceTelephoneNumber { get; set; }
    }
}
