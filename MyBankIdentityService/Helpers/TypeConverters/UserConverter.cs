using AutoMapper;
using MyBankIdentity.DTO;
using MyBankIdentityService.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MyBankIdentityService.Helpers.TypeConverters
{
    public class UserConverter : ITypeConverter<User, UserDTO>
    {
        public UserDTO Convert(User source, UserDTO destination, ResolutionContext context)
        {
            destination = new UserDTO
            {
                Id = source.Id,
                Email = source.Email,
                Fullname = source.Fullname,
                Username = source.Username,
                UserPrincipal = source.UserPrincipal,
                HrCode = source.HrCode,
                IsEnabled = source.IsEnabled,
                BranchCode = source.BranchCode
            };

            List<ModuleDTO> modules = source.Role.RoleModulesToPermissions.Select(m => new ModuleDTO
            {
                Id = m.Module.Id,
                Name = m.Module.Name,
                Permissions = source.Role.RoleModulesToPermissions.Where(a => a.ModuleId == m.Module.Id).Select(p => new PermissionDTO
                {
                    Id = p.Permission.Id,
                    Name = p.Permission.Name
                }).ToList()
            }).GroupBy(x => new { x.Id }).Select(x => x.First()).ToList();


            //List<PermissionDTO> permissions = modules.Select(p => new PermissionDTO
            //{
            //    Id = p.Permission.Id,
            //    Name = p.Permission.Name
            //}).ToList();



            destination.Role = new RoleDTO
            {
                Id = source.Role.Id,
                Name = source.Role.Name,
                Modules = modules
            };
            



            //// ======================================================
            //destination.Role = new RoleDTO
            //{
            //    Id = source.Role.Id,
            //    Name = source.Role.Name,
            //    Modules = source.Role.RoleModulesToPermissions.Select(m => new ModuleDTO
            //    {
            //        Id = m.Module.Id,
            //        Name = m.Module.Name,
            //        Permissions = source.Role.RoleModulesToPermissions.Select(p => new PermissionDTO
            //        {
            //            Id = p.Permission.Id,
            //            Name = p.Permission.Name
            //        }).ToList()
            //    }).Distinct().ToList()
            //};


            //destination.GlobalRole = new GlobalRoleDTO
            //{
            //    Id = source.GlobalRole.Id,
            //    Name = source.GlobalRole.Name,
            //    Roles = source.GlobalRole.GlobalRoleToRoles.Select(r => new RoleDTO
            //    {
            //        Id = r.Role.Id,
            //        Name = r.Role.Name,
            //        Modules = r.Role.RoleModulesToPermissions.Select(m => new ModuleDTO
            //        {
            //            Id = m.Module.Id,
            //            Name = m.Module.Name,
            //            Permissions = r.Role.RoleModulesToPermissions.Select(p => new PermissionDTO
            //            {
            //                Id = p.Permission.Id,
            //                Name = p.Permission.Name
            //            }).ToList()
            //        }).ToList()
            //    }).ToList()
            //};


            return destination;
        }
    }
}
