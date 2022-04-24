using Microsoft.EntityFrameworkCore;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ServiceModels;
using MyBankIdentityService.Reporitories.Interfaces;
using MyBankIdentityService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services
{
    public class ModuleService : GenericService<Module>, IModuleService
    {

        public ModuleService(IRepository<Module> repository) : base(repository)
        {
        }

        public async override Task<BaseServiceResponse> AddAsync(Module value)
        {
            if (value == null)
            {
                return new BaseServiceResponse(400, "Module can not be is null");
            }

            if (string.IsNullOrEmpty(value.Name?.Trim()))
            {
                return new BaseServiceResponse(400, "Module Name can not be empty or null");
            }

            return await base.AddAsync(value);
        }

        public override async Task<BaseServiceResponse> GetAllAsync()
        {
            try
            {
                var repoResponse = _repo.GetAsync().Include(m => m.Permissions).OrderBy(m => m.Name).ToList();
                return new BaseServiceResponse(200, repoResponse);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseServiceResponse(404, ex.Message);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }
    }
}
