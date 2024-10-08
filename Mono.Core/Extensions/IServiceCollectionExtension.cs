﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mono.Core.Accounts;
using Mono.Core.Authorization;
using Mono.Core.DirectPay;
using Mono.Core.LookUp;
using Mono.Core.Miscellaneous; // Add this using directive

namespace Mono.Core
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMono(this IServiceCollection services, Action<MonoInitializationOptions> options)
        {
            services.Configure(options);
            services.AddTransient(typeof(IRefitClientBuilder<>), typeof(RefitClientBuilder<>));
            services.AddTransient<IMonoAccounts, AccountService>();
            services.AddTransient<IMonoAuthorization, AuthorizationService>();
            services.AddTransient<IMonoDirectPay, DirectPayService>();
            services.AddTransient<IMonoMiscellaneous, MiscellaneousService>(); // Add this line
            services.AddTransient<IMonoLookUp, LookUpService>();
            return services;
        }

        public static IServiceCollection AddMonoAccounts(this IServiceCollection services, Action<MonoInitializationOptions> options)
        {
            services.Configure(options);
            services.AddTransient(typeof(IRefitClientBuilder<>), typeof(RefitClientBuilder<>));
            services.AddTransient<IMonoAccounts, AccountService>();
            return services;
        }

        public static IServiceCollection AddMonoAuthorization(this IServiceCollection services, Action<MonoInitializationOptions> options)
        {
            services.Configure(options);
            services.AddTransient(typeof(IRefitClientBuilder<>), typeof(RefitClientBuilder<>));
            services.AddTransient<IMonoAuthorization, AuthorizationService>();
            return services;
        }

        public static IServiceCollection AddMonoDirectPay(this IServiceCollection services, Action<MonoInitializationOptions> options)
        {
            services.Configure(options);
            services.AddTransient(typeof(IRefitClientBuilder<>), typeof(RefitClientBuilder<>));
            services.AddTransient<IMonoDirectPay, DirectPayService>();
            return services;
        }

        public static IServiceCollection AddMonoMiscellaneous(this IServiceCollection services, Action<MonoInitializationOptions> options)
        {
            services.Configure(options);
            services.AddTransient(typeof(IRefitClientBuilder<>), typeof(RefitClientBuilder<>));
            services.AddTransient<IMonoMiscellaneous, MiscellaneousService>();
            return services;
        }

        public static IServiceCollection AddMonoLookUp(this IServiceCollection services, Action<MonoInitializationOptions> options)
        {
            services.Configure(options);
            services.AddTransient(typeof(IRefitClientBuilder<>), typeof(RefitClientBuilder<>));
            services.AddTransient<IMonoLookUp, LookUpService>();
            return services;
        }

        
    }
}
