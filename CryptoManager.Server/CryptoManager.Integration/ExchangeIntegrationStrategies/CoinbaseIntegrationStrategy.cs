using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.Contracts.Integration.Utils;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;
using CryptoManager.Domain.IntegrationEntities.Exchanges.Coinbase;
using CryptoManager.Integration.Clients;
using CryptoManager.Integration.Utils;
using System;
using System.Threading.Tasks;

namespace CryptoManager.Integration.ExchangeIntegrationStrategies
{
    public class CoinbaseIntegrationStrategy : IExchangeIntegrationStrategy
    {
        private ICoinbaseIntegrationClient _coinbaseIntegrationClient;
        private readonly IExchangeIntegrationCache _cache;

        public ExchangesIntegratedType ExchangesIntegratedType => ExchangesIntegratedType.Coinbase;

        public CoinbaseIntegrationStrategy(IExchangeIntegrationCache cache, ICoinbaseIntegrationClient coinbaseIntegrationClient)
        {
            _cache = cache;
            _coinbaseIntegrationClient = coinbaseIntegrationClient;
        }

        public async Task<ObjectResult<TickerPriceDTO>> GetCurrentPriceAsync(string baseAssetSymbol, string quoteAssetSymbol)
        {
            var symbol = $"{baseAssetSymbol}-{quoteAssetSymbol}";
            var price = await _cache.GetAsync<TickerPrice>(ExchangesIntegratedType.Coinbase, ExchangeCacheEntityType.SymbolPrice, symbol);
            if (price == null)
            {
                price = await _coinbaseIntegrationClient.GetTickerPriceAsync(symbol);
                if(price == null)
                {
                    return ObjectResult<TickerPriceDTO>.Error($"symbol {symbol} does not exist in Coinbase");
                }
                await _cache.AddAsync(price, ExchangesIntegratedType.Coinbase, ExchangeCacheEntityType.SymbolPrice, symbol);
            }
            return ObjectResult<TickerPriceDTO>.Success(
                new TickerPriceDTO
                {
                    Symbol = symbol.Replace("-", string.Empty),
                    Price = decimal.Parse(price.Price)
                }
            );
        }

        public async Task<SimpleObjectResult> TestIntegrationUpAsync()
        {
            try
            { 
                var response = await _coinbaseIntegrationClient.GetTickerPriceAsync("BTC-GBP");
                if (response == null)
                {
                    return SimpleObjectResult.Error($"response = null");
                }
                return SimpleObjectResult.Success();
            }
            catch (Exception ex)
            {
                return SimpleObjectResult.Error(ex.Message);
            }
        }
    }
}
