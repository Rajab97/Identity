using MyBankIdentity.DTO;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ServiceModels;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services.Interfaces
{
    public interface IRoleModulesToPermissionService : IGenericService<RoleModulesToPermission>
    {
        Task<BaseServiceResponse> GetPermissionsAsync(int roleId);
        Task<BaseServiceResponse> AddAsync(RoleModulesToPermissionDTO model);
        Task<BaseServiceResponse> DeleteRoleModuleAsync(int roleId, int moduleId);
    }
}
