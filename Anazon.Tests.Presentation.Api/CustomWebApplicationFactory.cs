using Anazon.Infra.Data.Context;
using Anazon.Presentation.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Anazon.Tests.Presentation.Api
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AnazonContext>));
                if (descriptor is not null) services.Remove(descriptor);

                var inMemoryProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<AnazonContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDatabase");
                    options.UseInternalServiceProvider(inMemoryProvider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                var context = scope.ServiceProvider.GetRequiredService<AnazonContext>();

                context.Database.EnsureCreated();

                try
                {
                    Seed.PopulateUsers(context);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error on context seed: {ex.Message} {(ex.InnerException?.Message ?? string.Empty)}");
                }
            });
        }
    }
}
