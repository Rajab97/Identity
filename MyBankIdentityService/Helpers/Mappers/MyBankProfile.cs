using AutoMapper;
using MyBankIdentity.DTO;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Helpers.TypeConverters;
using System.DirectoryServices.AccountManagement;

namespace MyBankIdentityService.Helpers.Mappers
{
    public class MyBankProfile : Profile
    {

        public MyBankProfile()
        {
            //CreateMap<UserPrincipal, User>()
            //    .ForMember(dest => dest.Fullname, opt => opt.MapFrom(a => a.DisplayName))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(a => a.EmailAddress))
            //    .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(a => a.Enabled ?? false));

            CreateMap<Permission, PermissionDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(a => a.Name));

            CreateMap<Module, ModuleDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(a => a.Name));

            CreateMap<RoleModulesToPermission, RoleModulesToPermissionDTO>();

            //CreateMap<Role, RoleDTO>()
            //   .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
            //   .ForMember(dest => dest.Name, opt => opt.MapFrom(a => a.Name))
            //   .ForMember(dest => dest.Modules, opt => opt.MapFrom(a => a.RoleModulesToPermissions));

            //CreateMap<GlobalRole, GlobalRoleDTO>()
            //   .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
            //   .ForMember(dest => dest.Name, opt => opt.MapFrom(a => a.Name))
            //   .ForMember(dest => dest.Roles, opt => opt.MapFrom(a => a.GlobalRoleToRoles));
            
            //CreateMap<GlobalRoleToRole, GlobalRoleToRoleDTO>()
            //   .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
            //   .ForMember(dest => dest.GlobalRoleId, opt => opt.MapFrom(a => a.GlobalRoleId))
            //   .ForMember(dest => dest.RoleId, opt => opt.MapFrom(a => a.RoleId));

            //CreateMap<GlobalRoleToRoleDTO, GlobalRoleToRole>()
            //   .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
            //   .ForMember(dest => dest.GlobalRoleId, opt => opt.MapFrom(a => a.GlobalRoleId))
            //   .ForMember(dest => dest.RoleId, opt => opt.MapFrom(a => a.RoleId));


            //CreateMap<User, UserDTO>()
            //   .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Id))
            //   .ForMember(dest => dest.Email, opt => opt.MapFrom(a => a.Email))
            //   .ForMember(dest => dest.Fullname, opt => opt.MapFrom(a => a.Fullname))
            //   .ForMember(dest => dest.IsActive, opt => opt.MapFrom(a => a.IsActive));

            CreateMap<User, UserDTO>()
                .ConvertUsing<UserConverter>();



        }


        //private List<Role> GetRoles(List<GlobalRoleToRole> globalRoleToRoles)
        //{
        //    List<Role> list = new List<Role>();
        //    foreach (var item in globalRoleToRoles)
        //    {
        //        list.Add(item.Role);
        //    }
        //    return list;
        //}

    }
}
