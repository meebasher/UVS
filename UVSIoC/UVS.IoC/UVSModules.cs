using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UVS.Application.Queries;
using UVS.Domain.Interfaces;
using UVS.Infra.Data.Context;
using UVS.Infra.Data.Repository;


namespace UVS.IoC
{
    public static class UVSModules
    {
        public static void AddUVSServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddOptions();

            var connectionString = configuration.GetConnectionString("UVSContext");
            services.AddDbContext<UVSDbContext>(options => options.UseSqlite(connectionString));

            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(EmployeeGetQuery).Assembly));
            services.AddScoped<IUVSRepository, UVSRepository>();
        }

        public static async Task MigrateDbContextGenericAsync<T>(this IServiceProvider serviceProvider) where T : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            var dbServices = scope.ServiceProvider.GetRequiredService<T>();
#if DEBUG
            await dbServices.Database.EnsureDeletedAsync();
#endif
            await dbServices.Database.EnsureCreatedAsync();
        }
    }
}
