using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Data.Entities
{
    public class Role : BaseEntity
    {

        [StringLength(255)]
        public string Name { get; set; }


        public ICollection<RoleModulesToPermission> RoleModulesToPermissions { get; set; }
        //public ICollection<GlobalRoleToRole> GlobalRoleToRoles { get; set; }
    }
}
