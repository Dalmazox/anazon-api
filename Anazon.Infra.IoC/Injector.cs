using Anazon.Application.Services;
using Anazon.Domain.Interfaces.Services;
using Anazon.Domain.Interfaces.UoW;
using Anazon.Infra.Data.Context;
using Anazon.Infra.Data.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anazon.Infra.IoC
{
    public static class Injector
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext(configuration)
                .AddUnitOfWork()
                .AddServices();
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AnazonContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services
                .AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
