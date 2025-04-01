using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.IntegrationEntities.Exchanges.KuCoin;
using Refit;
namespace CryptoManager.Integration.Clients
{
    public interface IKuCoinIntegrationClient
    {
        [Get("/v1/market/allTickers")]
        Task<ResponseData<ResponseDataTicker>> GetTickersAsync();
    }
}
