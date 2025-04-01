using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.Contracts.Integration.Utils;
using CryptoManager.Domain.Entities;
using CryptoManager.Domain.IntegrationEntities.Exchanges;

namespace CryptoManager.Integration.Utils
{
    public interface IExchangeIntegrationCache
    {
        Task AddExchangeEntityAsync(Exchange exchange, ExchangesIntegratedType exchangeType);
        Task AddAsync<T>(IEnumerable<T> list, ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType) where T : class;
        Task AddAsync<T>(IEnumerable<T> list, ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType, Func<T, string> symbolSelector) where T : class;
        Task AddAsync<T>(T entity, ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType, string symbol) where T : class;
        Task<T> GetAsync<T>(ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType, string symbol) where T : class;
        Task<T> GetAsync<T>(ExchangesIntegratedType exchangeType, ExchangeCacheEntityType exchangeCacheEntityType) where T : class;
    }
}