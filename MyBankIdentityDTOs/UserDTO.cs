using System;
using System.Collections.Generic;
using System.Text;

namespace MyBankIdentityDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public string HrCode { get; set; }

        public bool IsActive { get; set; }

        public RoleDTO Role { get; set; }
    }
}
