using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encoding.Application;
using Encoding.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Encoding.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
           
            services.AddScoped<IEncoderService, EncoderService>();

            services.AddTransient<IEncodingStrategy, AdvanceEncodingStrategy>();

            return services;
        }

        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            return services;
        }
    }
}
