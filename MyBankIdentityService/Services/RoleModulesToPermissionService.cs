using AutoMapper;
using MyBankIdentity.DTO;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ServiceModels;
using MyBankIdentityService.Reporitories.Interfaces;
using MyBankIdentityService.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services
{
    public class RoleModulesToPermissionService : GenericService<RoleModulesToPermission>, IRoleModulesToPermissionService
    {

        private readonly IMapper _mapper;
        private readonly IRepository<Permission> _permissionRepo;

        public RoleModulesToPermissionService(IRepository<RoleModulesToPermission> repository, IMapper mapper, IRepository<Permission> permissionRepo) : base(repository)
        {
            _mapper = mapper;
            _permissionRepo = permissionRepo;
        }

        public async Task<BaseServiceResponse> GetPermissionsAsync(int roleId)
        {
            try
            {
                var repoResponse = _repo.GetAsync()
                                        .Where(a => a.RoleId == roleId)
                                        .Select(a => new
                                        {
                                            Id = a.Id,
                                            ModuleId = a.ModuleId,
                                            PermissionId = a.PermissionId,
                                            //Permission = new 
                                            //{
                                            //    Id = a.Permission.Id,
                                            //    Name = a.Permission.Name,
                                            //    Description = a.Permission.Description
                                            //},
                                            //Module = new 
                                            //{
                                            //    Id = a.Module.Id,
                                            //    Name = a.Module.Name
                                            //}
                                        })
                                        .ToList();

                //var repoResponse = _repo.GetAsync()
                //                        .Where(a => a.RoleId == roleId && a.ModuleId == moduleId)
                //                        .Select(a => new
                //                        {
                //                            Id = a.Id,
                //                            Permission = new Permission
                //                            {
                //                                Id = a.Permission.Id,
                //                                Name = a.Permission.Name
                //                            }
                //                        })
                //                        .ToList();

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

        public async Task<BaseServiceResponse> AddAsync(RoleModulesToPermissionDTO model)
        {
            try
            {
                var result = _repo.GetAsync()
                                  .FirstOrDefault(a =>
                                  a.RoleId == model.RoleId &&
                                  a.ModuleId == model.ModuleId &&
                                  a.PermissionId == model.PermissionId
                                  );
                if (result != null)
                {
                    return new BaseServiceResponse(400, "Permission already exist");
                }

                var permissionFromDb = _permissionRepo.GetAsync().FirstOrDefault(p => p.Id == model.PermissionId);
                if (permissionFromDb == null)
                {
                    return new BaseServiceResponse(400, $"Permission with id {model.PermissionId} is not exits");
                }
                else if(permissionFromDb.ModuleId != model.ModuleId)
                {
                    return new BaseServiceResponse(400, $"Permission with id {model.PermissionId} is not belong to module {model.ModuleId}");
                }

                var repoResponse = await _repo.AddAsync(new RoleModulesToPermission
                {
                    RoleId = model.RoleId,
                    ModuleId = model.ModuleId,
                    PermissionId = model.PermissionId
                });

                var output = _mapper.Map<RoleModulesToPermissionDTO>(repoResponse);

                return new BaseServiceResponse(201, output);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }

        public async Task<BaseServiceResponse> DeleteRoleModuleAsync(int roleId, int moduleId)
        {
            try
            {
                var entities = _repo.GetAsync()
                                        .Where(a => a.RoleId == roleId && a.ModuleId == moduleId)
                                        .ToList();

                var repoResponse = await _repo.DeleteRangeAsync(entities);

                return new BaseServiceResponse(200, repoResponse);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }
    }
}
