using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services.Interfaces
{
    public interface IPermissionService : IGenericService<Permission>
    {
        Task<BaseServiceResponse> GetAllAsync(int? moduleId);
    }
}
