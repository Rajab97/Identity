using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services.Interfaces
{
    public interface IRoleService : IGenericService<Role>
    {
        Task<BaseServiceResponse> GetRoleAsync(int roleId);
    }
}
