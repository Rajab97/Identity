using MyBankIdentityService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services.Interfaces
{
    public interface IActiveDirectoryService
    {
        bool ValidateCredentials(string username, string password);
        User GetUserFromAD(string username);
    }
}
