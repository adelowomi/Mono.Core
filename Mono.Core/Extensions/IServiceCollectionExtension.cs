using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mono.Core.Accounts; // Add this using directive

namespace Mono.Core
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMono(this IServiceCollection services, Action<MonoInitializationOptions> options)
        {
            services.Configure(options);
            services.AddTransient(typeof(IRefitClientBuilder<>), typeof(RefitClientBuilder<>));
            services.AddTransient<IMonoAccounts, AccountService>();
            return services;
        }

        public static IServiceCollection AddMonoAccounts(this IServiceCollection services, MonoInitializationOptions options)
        {
            services.Configure<MonoInitializationOptions>(o => o = options);
            services.AddTransient(typeof(IRefitClientBuilder<>), typeof(RefitClientBuilder<>));
            services.AddTransient<IMonoAccounts, AccountService>();
            return services;
        }

        
    }
}
