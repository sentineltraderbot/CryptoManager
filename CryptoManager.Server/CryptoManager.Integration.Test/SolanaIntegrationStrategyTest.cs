using System;
using System.Threading.Tasks;
using CryptoManager.Domain.DTOs;
using CryptoManager.Integration.BlockchainIntegrationStrategies;
using Solnet.Rpc;
using Xunit;

namespace CryptoManager.Integration.Test;

public class SolanaIntegrationStrategyTest
{
    [Fact]
    public async Task Should_Return_Balance_When_Valid_Wallet_Address_Provided_Async()
    {
        var strategy = new SolanaIntegrationStrategy(ClientFactory.GetClient(Cluster.MainNet));
        var balances = await strategy.GetBalancesAsync("GZx1AqsFVPVZqWhoVTp3FxUe8eez9aknaZ9TGKD8aGLb");
        Assert.True(balances.HasSucceded);
    }

    [Fact]
    public async Task Should_Return_Error_When_Invalid_Wallet_Address_Provided_Async()
    {
        var strategy = new SolanaIntegrationStrategy(ClientFactory.GetClient(Cluster.MainNet));
        var balances = await strategy.GetBalancesAsync("aaaaa");
        Assert.False(balances.HasSucceded);
    }

    [Fact(Skip = "only for local tests")]
    public async Task Should_Return_Succeded_When_Transfer_Async()
    {
        var strategy = new SolanaIntegrationStrategy(ClientFactory.GetClient(Cluster.MainNet));
        var response = await strategy.TransferTokenFundsAsync(
            new BlockchainTokenTransferDTO
            {
                SenderWalletAddress = "FiPhWKk6o16WP9Doe5mPBTxaBFXxdxRAW9BmodPyo9UK",
                //never commit this
                SenderPrivateKey = "",
                ReceiverWalletAddress = "6nKcdZ9NpiDhRs1CR4RPMVZQLZYfptcTymqp6quxHEpC",
                Amount = 1000000,
                Symbol = "SENTBOT"
            });
        Assert.True(response.HasSucceded);
    }
}
