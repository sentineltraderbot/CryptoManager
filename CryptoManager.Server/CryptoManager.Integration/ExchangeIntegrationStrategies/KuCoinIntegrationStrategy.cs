using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;
using CryptoManager.Domain.IntegrationEntities.Exchanges.KuCoin;
using CryptoManager.Integration.Clients;
using CryptoManager.Integration.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoManager.Integration.ExchangeIntegrationStrategies
{
    public class KuCoinIntegrationStrategy : IExchangeIntegrationStrategy, IExchangeIntegrationTickersStrategy
    {
        private readonly IKuCoinIntegrationClient _kuCoinIntegrationClient;
        private readonly IExchangeIntegrationCache _cache;

        public ExchangesIntegratedType ExchangesIntegratedType => ExchangesIntegratedType.KuCoin;

        public KuCoinIntegrationStrategy(IExchangeIntegrationCache cache, IKuCoinIntegrationClient kuCoinIntegrationClient)
        {
            _cache = cache;
            _kuCoinIntegrationClient = kuCoinIntegrationClient;
        }

        public async Task<ObjectResult<TickerPriceDTO>> GetCurrentPriceAsync(string baseAssetSymbol, string quoteAssetSymbol)
        {
            var symbol = $"{baseAssetSymbol}-{quoteAssetSymbol}";
            var price = await _cache.GetAsync<TickerPrice>(ExchangesIntegratedType.KuCoin, symbol);
            if (price == null)
            {
                var response = await _kuCoinIntegrationClient.GetTickersAsync();
                if(response.Data == null)
                {
                    return ObjectResult<TickerPriceDTO>.Error($"symbol {symbol} does not exist in KuCoin");
                }
                price = response.Data.Ticker.FirstOrDefault(a => a.Symbol.Equals(symbol));
                await _cache.AddAsync(response.Data.Ticker, ExchangesIntegratedType.KuCoin, a => a.Symbol);
                if(price == null)
                {
                    return ObjectResult<TickerPriceDTO>.Error($"symbol {symbol} does not exist in KuCoin");
                }
            }
            return ObjectResult<TickerPriceDTO>.Success(
                new TickerPriceDTO
                {
                    Symbol = symbol.Replace("-", string.Empty),
                    Price = decimal.Parse(price.Last)
                }
            );
        }

        public async Task<SimpleObjectResult> TestIntegrationUpAsync()
        {
            try
            {
                var response = await _kuCoinIntegrationClient.GetTickersAsync();
                if (response.Data == null)
                {
                    return SimpleObjectResult.Error($"response code: {response.Code}");
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
            var response = await _kuCoinIntegrationClient.GetTickersAsync();
            await _cache.AddAsync(response.Data.Ticker, ExchangesIntegratedType.Binance, a => a.Symbol);
            
            return response.Data.Ticker.Select(a => new TickerPriceDTO
            {
                Symbol = a.Symbol.Replace("-", string.Empty),
                Price = decimal.Parse(a.Last)
            });
        }
    }
}
