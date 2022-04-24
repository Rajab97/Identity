using MyBankIdentityService.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<BaseServiceResponse> AddAsync(T value);

        Task<BaseServiceResponse> GetAllAsync();

        Task<BaseServiceResponse> GetByIdAsync(int id);

        Task<BaseServiceResponse> UpdateAsync(T value);

        Task<BaseServiceResponse> DeleteAsync(int id);

    }
}
