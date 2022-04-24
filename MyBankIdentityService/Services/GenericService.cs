using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.ServiceModels;
using MyBankIdentityService.Reporitories.Interfaces;
using MyBankIdentityService.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity, new()
    {
        protected readonly IRepository<T> _repo;

        public GenericService(IRepository<T> repo)
        {
            _repo = repo;
        }

        public virtual async Task<BaseServiceResponse> AddAsync(T value)
        {
            try
            {
                var repoResponse = await _repo.AddAsync(value);
                return new BaseServiceResponse(201, repoResponse);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }

        public virtual async Task<BaseServiceResponse> DeleteAsync(int id)
        {
            try
            {
                var entity = _repo.GetAsync().FirstOrDefault(a => a.Id == id);
                var repoResponse = await _repo.DeleteAsync(entity);
                return new BaseServiceResponse(200, repoResponse);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseServiceResponse(404, ex.ParamName);
            }
            catch (Exception ex)
            {
                return new BaseServiceResponse(400, ex.Message);
            }
        }


        public virtual async Task<BaseServiceResponse> GetAllAsync()
        {
            try
            {
                var repoResponse = _repo.GetAsync().ToList();
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

        public virtual async Task<BaseServiceResponse> GetByIdAsync(int id)
        {
            try
            {
                var repoResponse = _repo.GetAsync().FirstOrDefault(x => x.Id == id);
                if (repoResponse == null)
                {
                    return new BaseServiceResponse(404);
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

        public virtual async Task<BaseServiceResponse> UpdateAsync(T value)
        {
            try
            {
                var repoResponse = await _repo.UpdateAsync(value);
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
