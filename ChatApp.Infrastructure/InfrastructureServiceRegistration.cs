
using Microsoft.Extensions.DependencyInjection;
using ChatApp.Domain.Repositories;
using ChatApp.Infrastructure.Repositories;

namespace ChatApp.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
        }
    }
}
