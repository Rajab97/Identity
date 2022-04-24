using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace MyBankRequestLogger
{
    public class MyBankRequestLoggerMiddleware
    {
        readonly RequestDelegate _next;
        private readonly MyBankRequestLoggerMiddlewareOptions _options;
        private readonly ILogger _logger;

        public MyBankRequestLoggerMiddleware(RequestDelegate next, IOptions<MyBankRequestLoggerMiddlewareOptions> options)
        {
            _next = next;
            _options = options.Value;
            _logger = _options.LoggerConfiguration.CreateLogger();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                UserDTO user = null;

                if (IsUserExistInContext(httpContext))
                {
                    user = JsonConvert.DeserializeObject<UserDTO>(httpContext.Items["User"].ToString());
                    _logger.Information($"Module: {_options.ModuleName} | User: {user?.UserPrincipal} | HTTP {httpContext.Request.Method} request to URL: {httpContext.Request.Path}");
                }

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }

        private bool IsUserExistInContext(HttpContext httpContext)
        {
            return httpContext.Items["User"] != null;
        }
    }


    internal class UserDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public string Username { get; set; }

        public string UserPrincipal { get; set; }

        public string Email { get; set; }

        public string HrCode { get; set; }

        public string BranchCode { get; set; }
    }
}
