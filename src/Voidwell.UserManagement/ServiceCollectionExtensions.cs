using Microsoft.Extensions.DependencyInjection;
using Voidwell.UserManagement.Services;

namespace Voidwell.UserManagement
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ISecurityQuestionService, SecurityQuestionService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddTransient<IUserHelper, UserHelper>();

            return services;
        }
    }
}
