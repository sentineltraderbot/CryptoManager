using CryptoManager.Domain.Contracts.Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoManager.Business
{
    public static class BusinessServiceCollectionExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISentinelTraderBotTokenService, SentinelTraderBotTokenService>();
            services.AddScoped<IRecaptchaService, RecaptchaService>();

            return services;
        }
    }
}
