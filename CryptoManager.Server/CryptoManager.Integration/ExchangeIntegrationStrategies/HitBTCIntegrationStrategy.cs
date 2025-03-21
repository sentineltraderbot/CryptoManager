using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.Contracts.Integration.Utils;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;
using CryptoManager.Domain.IntegrationEntities.Exchanges.HitBTC;
using CryptoManager.Integration.Clients;
using CryptoManager.Integration.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoManager.Integration.ExchangeIntegrationStrategies
{
    public class HitBTCIntegrationStrategy : IExchangeIntegrationStrategy
    {
        private readonly IHitBTCIntegrationClient _hitBTCIntegrationClient;
        private readonly IExchangeIntegrationCache _cache;

        public ExchangesIntegratedType ExchangesIntegratedType => ExchangesIntegratedType.HitBTC;

        public HitBTCIntegrationStrategy(IExchangeIntegrationCache cache, IHitBTCIntegrationClient hitBTCIntegrationClient)
        {
            _cache = cache;
            _hitBTCIntegrationClient = hitBTCIntegrationClient;
        }

        public async Task<ObjectResult<TickerPriceDTO>> GetCurrentPriceAsync(string baseAssetSymbol, string quoteAssetSymbol)
        {
            var symbol = $"{baseAssetSymbol}{quoteAssetSymbol}";
            var price = await _cache.GetAsync<TickerPrice>(ExchangesIntegratedType.HitBTC, ExchangeCacheEntityType.SymbolPrice, symbol);
            if (price == null)
            {
                var listPrices = await _hitBTCIntegrationClient.GetTickerPricesAsync();
                price = listPrices.FirstOrDefault(a => a.Symbol.Equals(symbol));
                await _cache.AddAsync(listPrices, ExchangesIntegratedType.HitBTC, ExchangeCacheEntityType.SymbolPrice, a => a.Symbol);
                if (price == null)
                {
                    return ObjectResult<TickerPriceDTO>.Error($"symbol {symbol} does not exist in HitBTC");
                }
            }
            return ObjectResult<TickerPriceDTO>.Success(
                new TickerPriceDTO
                {
                    Symbol = symbol,
                    Price = string.IsNullOrWhiteSpace(price.Last) ? decimal.Zero : decimal.Parse(price.Last)
                }
            );
        }

        public async Task<SimpleObjectResult> TestIntegrationUpAsync()
        {
            try
            {
                return await GetCurrentPriceAsync("BTC", "USD");
            }
            catch (Exception ex)
            {
                return SimpleObjectResult.Error(ex.Message);
            }
        }
    }
}
