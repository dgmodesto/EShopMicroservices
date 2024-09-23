using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data.Interceptors;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStirng = configuration.GetConnectionString("Database");
        
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();


        services.AddDbContext<ApplicationDbContext>((sp, options) => {
             options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
             options.UseSqlServer(connectionStirng,
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
         });

        //services.AddScopped<IApplicationDbContext, ApplicationDbContext>();

        /*
         Migration Commands
        - Add-Migration InitialCreate -OutputDir Data/Migrations -Project Ordering.Infrastructure -StartupProject Ordering.API
        - 
         */
        return services;
    }
}
