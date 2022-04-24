using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBankIdentityService.Data;
using MyBankIdentityService.Reporitories.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Reporitories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly MyBankIdentityContext _context;

        public Repository(MyBankIdentityContext context)
        {
            _context = context;
        }

        public virtual IQueryable<TEntity> GetAsync()
        {
            try
            {
                return _context.Set<TEntity>();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve data from database");
            }
        }
       

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{typeof(TEntity).Name} entity must not be null");
            }

            try
            {
                await _context.AddAsync(entity);
                var result = await Complete();

                return entity;
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null)
                {
                    var number = sqlException.Number;

                    if (number == 2627 || number == 2601)
                    {
                        throw new Exception($"{typeof(TEntity).Name} already exist");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw new Exception($"{typeof(TEntity).Name} could not be saved");
            }

            throw new Exception($"Something went wrong");
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{typeof(TEntity).Name} entity must not be null");
            }

            try
            {
                _context.Update(entity);
                var result = await Complete();

                return entity;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw new Exception($"{typeof(TEntity).Name} could not be updated");
            }
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {

            if (entity == null)
            {
                throw new ArgumentNullException($"{typeof(TEntity).Name} must not be null");
            }

            try
            {
                _context.Remove(entity);
                var result = await Complete();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null)
                {
                    var number = sqlException.Number;

                    if (number == 547)
                    {
                        throw new Exception($"{typeof(TEntity).Name} is used");
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception($"{typeof(TEntity).Name} could not be deleted");
            }

            throw new Exception($"Something went wrong");
        }

        public Task<int> Complete()
        {
            return _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"Entities must not be null");
            }

            try
            {
                _context.RemoveRange(entities);
                var result = await Complete();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw new Exception($"Entities could not be deleted");
            }
        }
    }
}
