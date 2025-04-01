using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Exchanges;

namespace CryptoManager.Domain.Contracts.Integration;

public interface IExchangeIntegrationTickersStrategy
{
    ExchangesIntegratedType ExchangesIntegratedType { get; }
    Task<IEnumerable<TickerPriceDTO>> GetTickersAsync();
}
