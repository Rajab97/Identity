using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models;
using MyBankIdentityService.Models.ServiceModels;
using MyBankIdentityService.Reporitories.Interfaces;
using MyBankIdentityService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services
{
    public class RoleService : GenericService<Role>, IRoleService
    {
        private readonly IMapper _mapper;

        public RoleService(IRepository<Role> repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task<BaseServiceResponse> GetRoleAsync(int roleId)
        {
            try
            {
                var repoResponse = _repo.GetAsync()
                                        .Include(r => r.RoleModulesToPermissions)
                                        .Where(a => a.Id == roleId)
                                        //.Select(a => a.RoleModulesToPermissions
                                        //    .Select(m => m.Module.Name)
                                        // )
                                        .Select(a => new RoleModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            //RoleModulesToPermissions = a.RoleModulesToPermissions.GroupBy(customer => customer.ModuleId).Select(group => group.First())
                                            Modules = a.RoleModulesToPermissions.Select(m => m.ModuleId).ToList()
                                        }).FirstOrDefault();
                if (repoResponse != null)
                {
                    repoResponse.Modules = repoResponse.Modules.Distinct().ToList();
                }
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

        public async override Task<BaseServiceResponse> AddAsync(Role value)
        {
            if (value == null)
            {
                return new BaseServiceResponse(400, "Role can not be is null");
            }

            if (string.IsNullOrEmpty(value.Name?.Trim()))
            {
                return new BaseServiceResponse(400, "Role Name can not be empty or null");
            }

            return await base.AddAsync(value);
        }


    }
}
