using System.Threading.Tasks;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;

namespace CryptoManager.Domain.Contracts.Integration
{
    public interface IExchangeIntegrationStrategy
    {
        ExchangesIntegratedType ExchangesIntegratedType { get; }
        Task<ObjectResult<TickerPriceDTO>> GetCurrentPriceAsync(string baseAssetSymbol, string quoteAssetSymbol);
        Task<SimpleObjectResult> TestIntegrationUpAsync();
    }
}