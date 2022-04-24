using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Utils
{
    public static class Constants
    {
        public static class ErrorMessage
        {
            public static readonly string UserNotFountAD = "User not found in Active Directory";
            public static readonly string UserNotActiveAD = "User not active in Active Directory";
            public static readonly string UserNotFountDB = "User not found in Database";
            public static readonly string UserAlreadyExist = "User already exist in MyBank";

        }


    }
}
