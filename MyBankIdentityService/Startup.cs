using System.Data.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyBankIdentityService.Data;
using MyBankIdentityService.Models.ConfigModels;
using MyBankIdentityService.Services;
using Serilog;
using Newtonsoft.Json;
using AutoMapper;
using MyBankIdentityService.Helpers.Mappers;
using MyBankIdentityService.Helpers;
using MyBankIdentityService.Reporitories.Interfaces;
using MyBankIdentityService.Reporitories.Implementations;
using MyBankIdentityService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using MyBankIdentity.Middleware;
using MyBankRequestLogger;

namespace MyBankIdentityService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            DbConfigModel dbConfig = new DbConfigModel();
            Configuration.Bind("DatabaseConfig", dbConfig);
            services.AddSingleton(dbConfig);

            JWTConfig jwtConfig = new JWTConfig();
            Configuration.Bind("JWTConfig", jwtConfig);
            services.AddSingleton(jwtConfig);

            CommonConfig commonConfig = new CommonConfig();
            Configuration.Bind("Common", commonConfig);
            services.AddSingleton(commonConfig);

            ActiveDirectoryConfig activeDirectoryConfig = new ActiveDirectoryConfig();
            Configuration.Bind("ActiveDirectoryConfig", activeDirectoryConfig);
            services.AddSingleton(activeDirectoryConfig);


            #region Register Internal Services
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
          
            services.AddTransient(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IModuleService, ModuleService>();
            services.AddTransient<IRoleModulesToPermissionService, RoleModulesToPermissionService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IBranchService, BranchService>();
            services.AddSingleton<IActiveDirectoryService, ActiveDirectoryService>();

            //services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<ITokenBuilder, JWTokenBuilder>();
            #endregion


            #region JWT Configuration
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = true;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("placeholder-key-that-is-long-enough-for-sha256")),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        RequireExpirationTime = false,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true
                    };
                });
            #endregion

            #region Register DbContext
            DbConnectionStringBuilder dbConnectionBuilder = new DbConnectionStringBuilder() {
                { "Data Source", dbConfig.DataSource },
                { "Initial Catalog", dbConfig.InitialCatalog },
                { "User ID", dbConfig.UserID },
                { "Password", dbConfig.Password }
            };
            services.AddDbContext<MyBankIdentityContext>(options =>
            {
                options.UseSqlServer(dbConnectionBuilder.ConnectionString);
            });
            #endregion

            #region Automapper Configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MyBankProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Register Swagger

            services.AddSwaggerGen(SwaggerGenerator.ConfigureSwaggerGen);

            #endregion

            services.AddMyBankIdentity(opt =>
            {
                opt.ModuleName = jwtConfig.Module;
                opt.Secret = jwtConfig.Secret;
            });

            services.AddMyBankRequestLogger(opt => { 
                opt.LoggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(Configuration, "SerilogRequestLogger");
                opt.ModuleName = jwtConfig.Module;
            });


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins("http://192.168.135.142", "http://vsrvmcrserv-test", "http://localhost:4200", "http://192.168.0.246", "http://mybank-web01-p")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("x-custom-header")
                    );
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
            Log.Information("MyBankIdentity Service started");

            app.UseCors("CorsPolicy");

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(SwaggerGenerator.ConfigureSwagger);
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(SwaggerGenerator.ConfigureSwaggerUI);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            // custom jwt auth middleware
            app.UseMyBankIdentity();
            app.UseAuthorization();

            app.UseMyBankRequestLogger();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
