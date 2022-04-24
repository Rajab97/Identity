using MyBankIdentity.DTO;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ServiceModels;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services.Interfaces
{
    public interface ITokenBuilder
    {
        Task<RefreshToken>  BuildRefreshToken();


        Task<BaseServiceResponse> AddAsync(RefreshToken value);

        Task<RefreshToken> GetByRefreshTokenAsync(string token);

        Task<RefreshToken> GetByUserIdAsync(int id);

        Task<BaseServiceResponse> UpdateAsync(RefreshToken value);

        Task<BaseServiceResponse> DeleteAsync(int id);
        Task<BaseServiceResponse> DeleteByUserIdAsync(int id);
        string BuildToken(UserDTO userDTO);
    }
}
