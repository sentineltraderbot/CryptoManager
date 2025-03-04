using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;
using CryptoManager.Domain.IntegrationEntities.Exchanges.BitcoinTrade;
using CryptoManager.Integration.Clients;
using CryptoManager.Integration.Utils;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CryptoManager.Integration.ExchangeIntegrationStrategies
{
    public class BitcoinTradeIntegrationStrategy : IExchangeIntegrationStrategy, IExchangeIntegrationTickersStrategy
    {
        private const int _numberOfRetries = 10;
        private readonly IBitcoinTradeIntegrationClient _bitcoinTradeIntegrationClient;
        private readonly IExchangeIntegrationCache _cache;

        public ExchangesIntegratedType ExchangesIntegratedType => ExchangesIntegratedType.BitcoinTrade;

        public BitcoinTradeIntegrationStrategy(IExchangeIntegrationCache cache, IBitcoinTradeIntegrationClient bitcoinTradeIntegrationClient)
        {
            _cache = cache;
            _bitcoinTradeIntegrationClient = bitcoinTradeIntegrationClient;
        }

        public async Task<ObjectResult<TickerPriceDTO>> GetCurrentPriceAsync(string baseAssetSymbol, string quoteAssetSymbol)
        {
            var symbol = $"{baseAssetSymbol}_{quoteAssetSymbol}";
            var price = await _cache.GetAsync<TickerPrice>(ExchangesIntegratedType.BitcoinTrade, symbol);
            if (price == null)
            {
                var response = await Policy
                    .Handle<ApiException>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
                    .RetryAsync(_numberOfRetries)
                    .ExecuteAsync(_bitcoinTradeIntegrationClient.GetTickersAsync);

                if(response.Data == null)
                {
                    return ObjectResult<TickerPriceDTO>.Error($"symbol {symbol} does not exist in Bitcointrade");
                }

                price = response.Data.FirstOrDefault(a => a.Pair.Equals(symbol));
                await _cache.AddAsync(response.Data, ExchangesIntegratedType.BitcoinTrade, a => a.Pair);
                if(price == null)
                {
                    return ObjectResult<TickerPriceDTO>.Error($"symbol {symbol} does not exist in Bitcointrade");
                }
            }
            return ObjectResult<TickerPriceDTO>.Success(
                new TickerPriceDTO
                {
                    Symbol = price.Pair.Replace("_", string.Empty),
                    Price = price.Ask
                } 
            );
        }

        public async Task<SimpleObjectResult> TestIntegrationUpAsync()
        {
            try
            {
                var response = await _bitcoinTradeIntegrationClient.GetTickersAsync();
                if (response.Data == null)
                {
                    return SimpleObjectResult.Error(response.Message);
                }
                return SimpleObjectResult.Success();
            }
            catch (Exception ex)
            {
                return SimpleObjectResult.Error(ex.Message);
            }
        }

        public async Task<IEnumerable<TickerPriceDTO>> GetTickersAsync()
        {
            var response = await Policy
                    .Handle<ApiException>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
                    .RetryAsync(_numberOfRetries)
                    .ExecuteAsync(_bitcoinTradeIntegrationClient.GetTickersAsync);
                    
            await _cache.AddAsync(response.Data, ExchangesIntegratedType.BitcoinTrade, a => a.Pair);
            
            return response.Data.Select(a => new TickerPriceDTO
            {
                Symbol = a.Pair.Replace("_", string.Empty),
                Price = a.Ask
            });
        }
    }
}
