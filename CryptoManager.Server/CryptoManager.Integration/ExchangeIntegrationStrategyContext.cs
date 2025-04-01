using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;

namespace CryptoManager.Integration
{
    public class ExchangeIntegrationStrategyContext : IExchangeIntegrationStrategyContext
    {
        private readonly IEnumerable<IExchangeIntegrationStrategy> _strategies;
        
        public ExchangeIntegrationStrategyContext(IEnumerable<IExchangeIntegrationStrategy> strategies)
        {
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        public Task<ObjectResult<TickerPriceDTO>> GetCurrentPriceAsync(string baseAssetSymbol, string quoteAssetSymbol, ExchangesIntegratedType exchangesIntegratedType)
        {
            var strategy = ResolveStrategy(exchangesIntegratedType);
            return strategy.GetCurrentPriceAsync(baseAssetSymbol, quoteAssetSymbol);
        }

        public Task<IEnumerable<TickerPriceDTO>> GetTickersAsync(ExchangesIntegratedType exchangesIntegratedType)
        {
            var strategy = ResolveStrategy(exchangesIntegratedType);
            if (strategy is not IExchangeIntegrationTickersStrategy tickersStrategy)
                throw new InvalidOperationException($"Invalid IntegrationType, invalidType={nameof(exchangesIntegratedType)} on ExchangeIntegrationStrategyContext TickersStrategy");
            return tickersStrategy.GetTickersAsync();
        }

        public Task<SimpleObjectResult> TestIntegrationUpAsync(ExchangesIntegratedType exchangesIntegratedType)
        {
            var strategy = ResolveStrategy(exchangesIntegratedType);
            return strategy.TestIntegrationUpAsync();
        }

        private IExchangeIntegrationStrategy ResolveStrategy(ExchangesIntegratedType exchangesIntegratedType)
        {
            var exchangeIntegrationStrategy = _strategies.FirstOrDefault(x => x.ExchangesIntegratedType == exchangesIntegratedType);
            if (exchangeIntegrationStrategy == null)
                throw new InvalidOperationException($"Invalid IntegrationType, invalidType={nameof(exchangesIntegratedType)} on ExchangeIntegrationStrategyContext");

            return exchangeIntegrationStrategy;
        }
    }
}
