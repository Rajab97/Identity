using AutoMapper;
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
    public class PermissionService : GenericService<Permission>, IPermissionService
    {
        public PermissionService(IRepository<Permission> repository) : base(repository)
        {
        }

        public async Task<BaseServiceResponse> GetAllAsync(int? moduleId)
        {
            try
            {
                var output = new List<Permission>();
                var repoResponse = _repo.GetAsync();
                if (!(moduleId is null) && moduleId != 0)
                {
                    repoResponse = repoResponse.Where(a => a.ModuleId == moduleId)
                                             .Select(a => new Permission
                                             {
                                                 Id = a.Id,
                                                 Name = a.Name,
                                                 Description = a.Description,
                                                 ModuleId = a.ModuleId
                                             });
                }
                output = repoResponse.ToList();
                return new BaseServiceResponse(200, output);
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
