using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Modules { get; set; }
    }
}
