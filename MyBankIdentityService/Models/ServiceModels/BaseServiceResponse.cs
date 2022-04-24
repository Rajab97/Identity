using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Models.ServiceModels
{
    public class BaseServiceResponse
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }

        public BaseServiceResponse()
        {
        }

        public BaseServiceResponse(int StatusCode)
        {
            this.StatusCode = StatusCode;
        }

        public BaseServiceResponse(int StatusCode, object Data)
        {
            this.StatusCode = StatusCode;
            this.Data = Data;
        }
    }
}
