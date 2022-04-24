using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBankRequestLogger
{
    public static class MyBankRequestLoggerMiddlewareExtensions
    {
        public static IServiceCollection AddMyBankRequestLogger(this IServiceCollection service, Action<MyBankRequestLoggerMiddlewareOptions> options = default)
        {
            options = options ?? (opts => { });

            service.Configure(options);
            return service;
        }

        public static IApplicationBuilder UseMyBankRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyBankRequestLoggerMiddleware>();
        }
    }
}
