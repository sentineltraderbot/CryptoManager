using CryptoManager.Domain.Contracts.Integration.Utils;
using CryptoManager.Domain.IntegrationEntities.Exchanges;
using CryptoManager.Domain.IntegrationEntities.Exchanges.KuCoin;
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
    public class KuCoinIntegrationStrategyTest
    {
        [Fact]
        public async Task Should_Return_Price_Async()
        {
            TickerPrice ticker = null;
            var symbol = "BTC-USDT";
            var cacheMock = new Mock<IExchangeIntegrationCache>(MockBehavior.Strict);
            cacheMock.Setup(repo => repo.GetAsync<TickerPrice>(ExchangesIntegratedType.KuCoin, ExchangeCacheEntityType.SymbolPrice, symbol))
                .ReturnsAsync(ticker);

            cacheMock.Setup(repo => repo.AddAsync(It.IsAny<IEnumerable<TickerPrice>>(), 
                                                  ExchangesIntegratedType.KuCoin, 
                                                  ExchangeCacheEntityType.SymbolPrice,
                                                  It.IsAny<Func<TickerPrice, string>>()))
                .Returns(Task.CompletedTask);

            var clientMock = new Mock<IKuCoinIntegrationClient>(MockBehavior.Strict);
            clientMock.Setup(c => c.GetTickersAsync())
                .ReturnsAsync(new ResponseData<ResponseDataTicker> { Code = "200", Data = new ResponseDataTicker { Ticker = [new TickerPrice { Last = "1", Symbol = symbol }] } });

            var strategy = new KuCoinIntegrationStrategy(cacheMock.Object, clientMock.Object);
            var price = await strategy.GetCurrentPriceAsync("BTC", "USDT");
            Assert.True(price.Item.Price > 0);
        }

        [Fact]
        public async Task Should_Return_Exception_When_Symbol_Not_Exists_In_Exchange_Async()
        {
            TickerPrice ticker = null;
            var symbol = "nuncatera-jsdhjkdhsajkdh";
            var cacheMock = new Mock<IExchangeIntegrationCache>(MockBehavior.Strict);
            cacheMock.Setup(repo => repo.GetAsync<TickerPrice>(ExchangesIntegratedType.KuCoin, ExchangeCacheEntityType.SymbolPrice, symbol))
                .ReturnsAsync(ticker);

            cacheMock.Setup(repo => repo.AddAsync(It.IsAny<IEnumerable<TickerPrice>>(), 
                                                  ExchangesIntegratedType.KuCoin,
                                                  ExchangeCacheEntityType.SymbolPrice,
                                                  It.IsAny<Func<TickerPrice, string>>()))
                .Returns(Task.CompletedTask);

            var clientMock = new Mock<IKuCoinIntegrationClient>(MockBehavior.Strict);
            clientMock.Setup(c => c.GetTickersAsync())
                .ReturnsAsync(new ResponseData<ResponseDataTicker> { Code = "200", Data = null });

            var strategy = new KuCoinIntegrationStrategy(cacheMock.Object, clientMock.Object);
            var result = await strategy.GetCurrentPriceAsync("nuncatera", "jsdhjkdhsajkdh");
            Assert.False(result.HasSucceded);
            Assert.Equal($"symbol {symbol} does not exist in KuCoin", result.ErrorMessage);
        }
    }
}
