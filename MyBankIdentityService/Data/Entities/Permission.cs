using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Data.Entities
{
    public class Permission : BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int ModuleId { get; set; }

        public Module Module { get; set; }
    }
}
