using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.IntegrationEntities.Exchanges.BitcoinTrade;
using Refit;
namespace CryptoManager.Integration.Clients
{
    public interface IBitcoinTradeIntegrationClient
    {
        [Get("/v4/public/tickers")]
        Task<ResponseData<IEnumerable<TickerPrice>>> GetTickersAsync();
    }
}
