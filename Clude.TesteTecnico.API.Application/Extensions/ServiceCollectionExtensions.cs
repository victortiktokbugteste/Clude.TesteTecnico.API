using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Clude.TesteTecnico.API.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
} 