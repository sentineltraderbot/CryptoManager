using CryptoManager.Domain.IntegrationEntities.Exchanges;
using CryptoManager.Domain.IntegrationEntities.Exchanges.HitBTC;
using CryptoManager.Integration.Clients;
using CryptoManager.Integration.ExchangeIntegrationStrategies;
using CryptoManager.Integration.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CryptoManager.Integration.Test
{
    public class HitBTCIntegrationStrategyTest
    {
        private const string HITBTC_API = "https://api.hitbtc.com/api/";

        [Fact]
        public async Task Should_Return_Price_Async()
        {
            TickerPrice ticker = null;
            var symbol = "LTCBTC";
            var cacheMock = new Mock<IExchangeIntegrationCache>(MockBehavior.Strict);
            cacheMock.Setup(repo => repo.GetAsync<TickerPrice>(ExchangesIntegratedType.HitBTC, symbol))
                .ReturnsAsync(ticker);

            cacheMock.Setup(repo => repo.AddAsync(It.IsAny<IEnumerable<TickerPrice>>(), ExchangesIntegratedType.HitBTC, It.IsAny<Func<TickerPrice, string>>()))
                .Returns(Task.CompletedTask);

            var clientMock = new Mock<IHitBTCIntegrationClient>(MockBehavior.Strict);
            clientMock.Setup(c => c.GetTickerPricesAsync())
                .ReturnsAsync(new[] { new TickerPrice { Last = "1", Symbol = symbol } });

            var strategy = new HitBTCIntegrationStrategy(cacheMock.Object, clientMock.Object);
            var price = await strategy.GetCurrentPriceAsync("LTC", "BTC");
            Assert.True(price.Item.Price > 0);
        }


        [Fact]
        public async Task Should_Return_Exception_When_Symbol_Not_Exists_In_Exchange_Async()
        {
            TickerPrice ticker = null;
            var symbol = "nuncaterajsdhjkdhsajkdh";
            var cacheMock = new Mock<IExchangeIntegrationCache>(MockBehavior.Strict);
            cacheMock.Setup(repo => repo.GetAsync<TickerPrice>(ExchangesIntegratedType.HitBTC, symbol))
                .ReturnsAsync(ticker);

            cacheMock.Setup(repo => repo.AddAsync(It.IsAny<IEnumerable<TickerPrice>>(),
                                                  ExchangesIntegratedType.HitBTC,
                                                  It.IsAny<Func<TickerPrice, string>>()))
                .Returns(Task.CompletedTask);

            var clientMock = new Mock<IHitBTCIntegrationClient>(MockBehavior.Strict);
            clientMock.Setup(c => c.GetTickerPricesAsync())
                .ReturnsAsync(new[] { new TickerPrice { Last = "1", Symbol = "abc" } });

            var strategy = new HitBTCIntegrationStrategy(cacheMock.Object, clientMock.Object);
            var result = await strategy.GetCurrentPriceAsync("nuncatera", "jsdhjkdhsajkdh");
            Assert.False(result.HasSucceded);
            Assert.Equal($"symbol {symbol} not exists in HitBTC", result.ErrorMessage);
        }
    }
}
