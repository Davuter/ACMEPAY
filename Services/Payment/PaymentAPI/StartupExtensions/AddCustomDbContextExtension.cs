using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PaymentAPI.StartupExtensions
{
    public static class CustomDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PaymentDbContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            },
                       ServiceLifetime.Scoped
                   );

            var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>()
             .UseSqlServer(configuration["ConnectionString"]);

            using var dbContext = new PaymentDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();

            //if (dbContext.Database.IsRelational())
            //    dbContext.Database.Migrate();

            return services;
        }
    }

}
