using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models.ConfigModels
{
    public class JWTConfig
    {
        private string _decryptedSecret;

        public string Module { get; set; }
        public string Secret { get; set; }

        public int ExpireWithSeconds { get; set; }
    }
}
