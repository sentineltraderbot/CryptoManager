using System;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.Contracts.Integration.Utils;
using CryptoManager.Domain.Contracts.Repositories;
using CryptoManager.Domain.Entities;
using CryptoManager.Domain.IntegrationEntities.Exchanges;
using CryptoManager.Integration.BlockchainIntegrationStrategies;
using CryptoManager.Integration.Clients;
using CryptoManager.Integration.ExchangeIntegrationStrategies;
using CryptoManager.Integration.Utils;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Solnet.Rpc;

namespace CryptoManager.Integration
{
    public static class IntegrationServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrations(this IServiceCollection services)
        {
            //exchange integration strategies
            services.AddScoped<IExchangeIntegrationCache, ExchangeIntegrationCache>();
            services.AddScoped<IExchangeIntegrationStrategyContext, ExchangeIntegrationStrategyContext>();
            services.AddScoped<IExchangeIntegrationStrategy, BinanceIntegrationStrategy>();
            services.AddScoped<IExchangeIntegrationStrategy, BitcoinTradeIntegrationStrategy>();
            services.AddScoped<IExchangeIntegrationStrategy, CoinbaseIntegrationStrategy>();
            services.AddScoped<IExchangeIntegrationStrategy, HitBTCIntegrationStrategy>();
            services.AddScoped<IExchangeIntegrationStrategy, KuCoinIntegrationStrategy>();
            services.AddScoped<IExchangeIntegrationTickersStrategy, KuCoinIntegrationStrategy>();
            services.AddScoped<IExchangeIntegrationTickersStrategy, BitcoinTradeIntegrationStrategy>();
            services.AddScoped<IExchangeIntegrationTickersStrategy, BinanceIntegrationStrategy>();

            //blockchain integration strategies
            services.AddScoped<IBlockchainIntegrationStrategyContext, BlockchainIntegrationStrategyContext>();
            services.AddScoped<IBlockchainIntegrationStrategy, SolanaIntegrationStrategy>();

            //solana blockchain client
            services.AddSingleton(sp =>
                ClientFactory.GetClient(Cluster.MainNet)
            );

            //clients
            services.AddRefitClient<IBinanceIntegrationClient>()
                .ConfigureHttpClient(async (sp, c) => await AddHttpClient(sp, c, ExchangesIntegratedType.Binance));

            services.AddRefitClient<IBitcoinTradeIntegrationClient>()
                .ConfigureHttpClient(async (sp, c) => await AddHttpClient(sp, c, ExchangesIntegratedType.BitcoinTrade));

            services.AddRefitClient<ICoinbaseIntegrationClient>()
                .ConfigureHttpClient(async (sp, c) => await AddHttpClient(sp, c, ExchangesIntegratedType.Coinbase));

            services.AddRefitClient<IHitBTCIntegrationClient>()
                .ConfigureHttpClient(async (sp, c) => await AddHttpClient(sp, c, ExchangesIntegratedType.HitBTC));

            services.AddRefitClient<IKuCoinIntegrationClient>()
                .ConfigureHttpClient(async (sp, c) => await AddHttpClient(sp, c, ExchangesIntegratedType.KuCoin));

            services.AddRefitClient<IRecaptchaClient>()
                .ConfigureHttpClient((c) =>
                    c.BaseAddress = new Uri("https://www.google.com/")
                );

            return services;
        }

        private async static Task AddHttpClient(IServiceProvider serviceProvider, HttpClient httpClient, ExchangesIntegratedType exchangeType)
        {
            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using var serviceScope = serviceScopeFactory.CreateScope();
            var cache = serviceScope.ServiceProvider.GetService<IExchangeIntegrationCache>();
            
            var exchange = await cache.GetAsync<Exchange>(exchangeType, ExchangeCacheEntityType.ExchangeEntity);
            if(exchange == null)
            {
                var exchangeRepository = serviceScope.ServiceProvider.GetService<IExchangeRepository>();
                exchange = await exchangeRepository.GetByExchangeTypeAsync(exchangeType);
                await cache.AddExchangeEntityAsync(exchange, exchangeType);
            }

            if (exchange != null)
            {
                httpClient.BaseAddress = new Uri(exchange.APIUrl);
            }
        }
    }
}
