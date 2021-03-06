using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Data.Entities
{
    public class Module : BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }

        public ICollection<Permission> Permissions { get; set; }

    }
}
