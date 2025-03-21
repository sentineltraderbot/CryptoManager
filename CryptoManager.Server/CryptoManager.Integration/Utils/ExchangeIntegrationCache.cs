using CryptoManager.Domain.Contracts.Integration.Utils;
using CryptoManager.Domain.Entities;
using CryptoManager.Domain.IntegrationEntities.Exchanges;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CryptoManager.Integration.Utils
{
    public class ExchangeIntegrationCache : IExchangeIntegrationCache
    {
        private readonly Dictionary<ExchangeCacheEntityType, DistributedCacheEntryOptions> _cacheOptions;
        private readonly IDistributedCache _cache;
        public ExchangeIntegrationCache(IDistributedCache cache)
        {
            _cache = cache;
            _cacheOptions = new Dictionary<ExchangeCacheEntityType, DistributedCacheEntryOptions>
            {
                {
                    ExchangeCacheEntityType.ExchangeEntity,
                    new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1))
                },
                {   
                    ExchangeCacheEntityType.SymbolPrice,
                    new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(1))
                },
                {
                    ExchangeCacheEntityType.SymbolPriceList,
                    new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(2))
                }
            };
        }

        public async Task AddExchangeEntityAsync(Exchange exchange, ExchangesIntegratedType exchangeType)
        {
            string key = GenerateKey(exchangeType, ExchangeCacheEntityType.ExchangeEntity, null);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(exchange), _cacheOptions[ExchangeCacheEntityType.ExchangeEntity]);
        }

        public async Task AddAsync<T>(T entity, ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType, string symbol) where T : class
        {
            string key = GenerateKey(exchangeType, exchangeCacheEntityType, symbol);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(entity), _cacheOptions[exchangeCacheEntityType]);
        }

        public async Task AddAsync<T>(IEnumerable<T> list, ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType, Func<T, string> symbolSelector) where T : class
        {
            foreach (T item in list)
            {
                await AddAsync(item, exchangeType, exchangeCacheEntityType, symbolSelector(item));
            }
        }

        public async Task AddAsync<T>(IEnumerable<T> list, ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType) where T : class
        {
            string key = GenerateKey(exchangeType, exchangeCacheEntityType, null);
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(list), _cacheOptions[exchangeCacheEntityType]);
        }

        public async Task<T> GetAsync<T>(ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType, string symbol) where T : class
        {
            string key = GenerateKey(exchangeType, exchangeCacheEntityType, symbol);
            var value = await _cache.GetStringAsync(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public Task<T> GetAsync<T>(ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType) where T : class
        {
            return GetAsync<T>(exchangeType, exchangeCacheEntityType, null);
        }

        private string GenerateKey(ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType, string symbol)
        {
            return $"{exchangeType}/{exchangeCacheEntityType}/{symbol}";
        }
    }
}
