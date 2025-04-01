
using System.Collections.Generic;

namespace CryptoManager.Domain.IntegrationEntities.Exchanges.KuCoin
{
    public class ResponseData<T>
    {
        public string Code { get; set; }
        public T Data { get; set; }
    }

    public class ResponseDataTicker
    {
        public string Symbol { get; set; }
        public IEnumerable<TickerPrice> Ticker { get; set; }
    }
}
