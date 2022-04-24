using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Data.Entities
{
    public class RoleModulesToPermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int? PermissionId { get; set; }

        public Role Role{ get; set; }
        public Module Module { get; set; }
        public Permission Permission { get; set; }

    }
}
