using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CryptoManager.Domain.Contracts.Business;
using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Blockchains;
using Microsoft.Extensions.Configuration;

namespace CryptoManager.Business;

public class SentinelTraderBotTokenService : ISentinelTraderBotTokenService
{
    private const decimal _airdropAmount = 1000;
    private const string Symbol = "SENTBOT";
    private const string _airdropWalletAddress = "GZx1AqsFVPVZqWhoVTp3FxUe8eez9aknaZ9TGKD8aGLb";
    private const string _airdropWalletPrivateKeySecretName = "airdrop-solana-wallet-private-key";
    public readonly IBlockchainIntegrationStrategyContext _blockchainIntegrationStrategyContext;
    private readonly IConfiguration _configuration;
    private readonly IAccountService _accountService;

    public SentinelTraderBotTokenService(IBlockchainIntegrationStrategyContext blockchainIntegrationStrategyContext,
        IConfiguration configuration,
        IAccountService accountService)
    {
        _blockchainIntegrationStrategyContext = blockchainIntegrationStrategyContext;
        _configuration = configuration;
        _accountService = accountService;
    }

    public Task<ObjectResult<IEnumerable<TickerBalanceDTO>>> GetBalancesAsync(string walletAddress)
    {
        return _blockchainIntegrationStrategyContext.GetBalancesAsync(walletAddress, BlockchainIntegratedTypes.Solana);
    }

    public async Task<SimpleObjectResult> SentinelTraderTokenAirdropAsync(string walletAddress, Guid userId)
    {
        var user = await _accountService.GetUserAsync(userId);
        if(user.HasReceivedAirdrop)
        {
            return SimpleObjectResult.Error("You've already received an airdrop");
        }

        var keyVaultUrl = _configuration["AzureKeyVault:Url"];
        var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

        var secret = await client.GetSecretAsync(_airdropWalletPrivateKeySecretName);
        var transferDto = new BlockchainTokenTransferDTO
        {
            Amount = _airdropAmount,
            ReceiverWalletAddress = walletAddress,
            SenderPrivateKey = secret.Value.Value,
            SenderWalletAddress = _airdropWalletAddress,
            Symbol = Symbol
        };
        var response = await _blockchainIntegrationStrategyContext.TransferTokenFundsAsync(transferDto, BlockchainIntegratedTypes.Solana);
        if (!response.HasSucceded)
        {
            return SimpleObjectResult.Error($"Error transferring funds: {response.ErrorMessage}");
        }
        user.HasReceivedAirdrop = true;
        user.SolanaWalletAddress = walletAddress;
        return await _accountService.UpdateUserAsync(user);
    }
}
