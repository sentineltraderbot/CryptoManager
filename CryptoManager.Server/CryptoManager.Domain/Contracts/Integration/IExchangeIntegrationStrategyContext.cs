using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;

namespace CryptoManager.Domain.Contracts.Integration
{
    public interface IExchangeIntegrationStrategyContext
    {
        Task<ObjectResult<TickerPriceDTO>> GetCurrentPriceAsync(string baseAssetSymbol, string quoteAssetSymbol, ExchangesIntegratedType exchangesIntegratedType);
        Task<IEnumerable<TickerPriceDTO>> GetTickersAsync(ExchangesIntegratedType exchangesIntegratedType);
        Task<SimpleObjectResult> TestIntegrationUpAsync(ExchangesIntegratedType exchangesIntegratedType);
    }
}