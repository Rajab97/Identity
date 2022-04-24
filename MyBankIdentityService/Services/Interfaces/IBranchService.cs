using GenericModelsLibrary.DTO.Common;
using MyBankIdentityService.Data.Entities;
using MyBankIdentityService.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBankIdentityService.Services.Interfaces
{
    public interface IBranchService
    {
        public Task<GenericResponseModel<int>> SumbitBranchModel(KafkaBranchMessageValueModel model);
        public Task<GenericResponseModel<List<Branch>>> GetBranches();
    }
}
