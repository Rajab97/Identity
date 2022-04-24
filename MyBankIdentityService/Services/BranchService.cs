using GenericModelsLibrary.DTO.Common;
using Microsoft.EntityFrameworkCore;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.DTO;
using MyBankIdentityService.Reporitories.Interfaces;
using MyBankIdentityService.Services.Interfaces;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services
{
    public class BranchService : IBranchService
    {
        private readonly IRepository<Branch> branchRepository;

        public BranchService(IRepository<Branch> branchRepository)
        {
            this.branchRepository = branchRepository;
        }

        public async Task<GenericResponseModel<int>> SumbitBranchModel(KafkaBranchMessageValueModel model)
        {
            var result = new GenericResponseModel<int>();
            try
            {
                Log.Information($"requested model from kafkaConsumer: {JsonConvert.SerializeObject(model)}");
                if (string.IsNullOrWhiteSpace(model?.Code))
                {
                    Log.Warning("Branch model which was submited from kafka is not right");
                    result.Error("Branch model is not right", 400);
                }
                else
                {
                    var exist = branchRepository.GetAsync().FirstOrDefault(b => b.Code == model.Code);
                    if (exist == null && !model.Disable)
                    {
                        Log.Information("Submited branch is new and it is inserting...");
                        exist = new Branch
                        {
                            Code = model.Code,
                            Name = model.NameEng,
                            ManagerHrCode = model.ManagerHrCode,
                            CreatedDate = DateTime.Now
                        };
                        await branchRepository.AddAsync(exist);
                        await branchRepository.Complete();

                        Log.Information($"New branch successfully added to the DB. Id: {exist.Id}");
                        result.Success(exist.Id);
                    }
                    else if(exist != null)
                    {
                        var branchId = exist.Id;
                        Log.Information("Submited branch exists");
                        if (!model.Disable)
                        {
                            Log.Information("Branch is updating...");
                            exist.Code = model.Code;
                            exist.Name = model.NameEng;
                            exist.ManagerHrCode = model.ManagerHrCode;
                            exist.LastUpdateDate = DateTime.Now;

                            await branchRepository.Complete();

                            Log.Information($"Exist branch successfully updated. Id: {branchId}");
                        }
                        else
                        {
                            Log.Information("Branch is deleting...");

                            await branchRepository.DeleteAsync(exist);
                            await branchRepository.Complete();

                            Log.Information($"Exist branch successfully deleted. Id: {branchId}");
                            result.Success(branchId);
                        }
                        result.Success(branchId);
                    }
                    else
                    {
                        Log.Information("There is not such a branch to delete from DB");
                        result.Success(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occured during branch submission");
                result.InternalError();
            }
            return result;
        }

        public async Task<GenericResponseModel<List<Branch>>> GetBranches()
        {
            var result = new GenericResponseModel<List<Branch>>();
            try
            {
                var branches = await branchRepository.GetAsync().ToListAsync();
                result.Success(branches);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occured during getting branches");
                result.InternalError();
            }
            return result;
        }
    }
}
