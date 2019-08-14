using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Voidwell.UserManagement.Data;
using Voidwell.UserManagement.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System;
using Voidwell.UserManageement.Api.Filters;
using Voidwell.UserManagement;
using Voidwell.Logging;

namespace Voidwell.UserManageement.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);

            if (env.IsDevelopment())
            {
                builder.AddJsonFile("devsettings.json", true, true);
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddMvcOptions(o =>
                {
                    o.Filters.AddService(typeof(InvalidSecurityQuestionFilter));
                    o.Filters.AddService(typeof(InvalidUserIdFilter));
                    o.Filters.AddService(typeof(InvalidPasswordFilter));
                    o.Filters.AddService(typeof(InvalidRoleRequestFilter));
                    o.Filters.AddService(typeof(UserLockedOutFilter));
                });

            services.AddEntityFrameworkContext(Configuration);

            services.AddIdentity<ApplicationUser, ApplicationRole>(identity =>
                {
                    identity.User.RequireUniqueEmail = true;
                    identity.Password.RequireDigit = false;
                    identity.Password.RequireNonAlphanumeric = false;
                    identity.Password.RequireLowercase = false;
                    identity.Password.RequireUppercase = false;
                    identity.Password.RequiredLength = 6;

                    identity.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    identity.Lockout.MaxFailedAccessAttempts = 5;
                })
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = "http://voidwellauth:5000";
                o.Audience = "voidwell-usermanagement";
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false
                };

                var validator = o.SecurityTokenValidators.OfType<JwtSecurityTokenHandler>().SingleOrDefault();
                validator.InboundClaimTypeMap = new Dictionary<string, string>();
                validator.OutboundClaimTypeMap = new Dictionary<string, string>();
            });

            services.AddApplicationServices();

            services.AddTransient<InvalidSecurityQuestionFilter>();
            services.AddTransient<InvalidUserIdFilter>();
            services.AddTransient<InvalidPasswordFilter>();
            services.AddTransient<InvalidRoleRequestFilter>();
            services.AddTransient<UserLockedOutFilter>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.InitializeDatabases();

            app.UseLoggingMiddleware();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
