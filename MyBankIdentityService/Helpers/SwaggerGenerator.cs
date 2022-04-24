using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyBankIdentityService.Helpers
{
    public class SwaggerGenerator
    {
        public static void ConfigureSwaggerGen(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "MyBank Identity Service",
                Description = "MyBank Identity Service API for auth procedures"
            });

            swaggerGenOptions.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
            {
                Description = "Please enter API Key ",
                In = ParameterLocation.Header, // where to find apiKey, probably in a header
                Name = "ApiKey", //header with api key
                Type = SecuritySchemeType.ApiKey, // this value is always "apiKey"
            });

            OpenApiSecurityRequirement openApiSecurityRequirement = new OpenApiSecurityRequirement();
            openApiSecurityRequirement.Add(new OpenApiSecurityScheme()
            {
                Description = "Please enter API Key ",
                In = ParameterLocation.Header, // where to find apiKey, probably in a header
                Name = "ApiKey", //header with api key
                Type = SecuritySchemeType.ApiKey, // this value is always "apiKey"
            }, new List<string>());

            swaggerGenOptions.AddSecurityRequirement(openApiSecurityRequirement);
        }

        public static void ConfigureSwagger(SwaggerOptions swaggerOptions)
        {
            swaggerOptions.RouteTemplate = $"swagger/" + "{documentName}/swagger.json";
        }
        public static void ConfigureSwaggerUI(SwaggerUIOptions swaggerUIOptions)
        {
            swaggerUIOptions.RoutePrefix = $"swagger";
            //var swaggerJsonBasePath = string.IsNullOrWhiteSpace(swaggerUIOptions.RoutePrefix) ? "." : "..";
            swaggerUIOptions.SwaggerEndpoint($"v1/swagger.json", $"");
        }


    }
}
