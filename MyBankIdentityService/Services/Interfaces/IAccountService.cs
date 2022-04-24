using MyBankIdentityService.Models;
using MyBankIdentityService.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseServiceResponse> Register(RegisterModel registerModel);
        string TestTokenGenerator();

        Task<BaseServiceResponse> UpdateUsersBranchInformation();
        Task<BaseServiceResponse> Login(AuthenticateRequest model);
        Task<BaseServiceResponse> Logout(int id);
        Task<BaseServiceResponse> RefreshToken(string token);
        bool RevokeToken(string token, string ipAddress);

        Task<BaseServiceResponse> EditRole (UserRoleModel model);
        Task<BaseServiceResponse> EditBranch (UserBranchModel model);
        //Task<BaseServiceResponse> Users();
        Task<BaseServiceResponse> Users(UsersQueryParam query = null);
        Task<BaseServiceResponse> ChangeActivation(UserStatusModel model);
    }
}
