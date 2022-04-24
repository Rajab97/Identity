using System.ComponentModel.DataAnnotations;

namespace MyBankIdentityService.Data.Entities
{
    public class User : BaseEntity
    {
        [StringLength(255)]
        public string Fullname { get; set; }

        [StringLength(255)]
        public string Username { get; set; }

        [StringLength(255)]
        [Required]
        public string UserPrincipal { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(10)]
        public string HrCode { get; set; }

        [StringLength(100)]
        public string BranchCode { get; set; }
      

        public bool IsEnabled { get; set; }

        public int RoleId { get; set; }


        public Role Role { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
