using System;
using System.ComponentModel.DataAnnotations;

namespace MyBankIdentityService.Data.Entities
{
    public class Branch : BaseEntity
    {
        [StringLength(255)]
        public string Code { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string ManagerHrCode { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
