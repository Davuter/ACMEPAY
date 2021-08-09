using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Payment.Core.Application.CQRS.Command.Authorize;
using Payment.Core.Application.Mapping;
using Payment.Infrastructure.Repositories;
using Payment.Core.Domain.Interfaces;
using FluentValidation.AspNetCore;
using Payment.Core.Application.Infrastructure;
using PaymentAPI.Filter;
using Payment.Core.Application.CQRS.Command.Void;

namespace PaymentAPI.StartupExtensions
{
    public static class AddCustomApplicationServices
    {
        public static IServiceCollection AddApplicationServiceRegistration(this IServiceCollection services)
        {

            services.AddMediatR(typeof(AuthorizeCommandHandler).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperGeneralMapping).GetTypeInfo().Assembly });

            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));


            services
            .AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiValidationAttribute));
                options.Filters.Add(typeof(CustomExceptionFilter));
            }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(VoidCommandValidator).GetTypeInfo().Assembly));

            return services;
        }
    }
}
