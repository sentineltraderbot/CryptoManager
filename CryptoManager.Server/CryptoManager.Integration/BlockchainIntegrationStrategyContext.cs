using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoManager.Domain.Contracts.Integration;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Blockchains;

namespace CryptoManager.Integration;

public class BlockchainIntegrationStrategyContext : IBlockchainIntegrationStrategyContext
{
    private readonly IEnumerable<IBlockchainIntegrationStrategy> _strategies;

    public BlockchainIntegrationStrategyContext(IEnumerable<IBlockchainIntegrationStrategy> strategies)
    {
        _strategies = strategies;
    }

    public Task<ObjectResult<IEnumerable<TickerBalanceDTO>>> GetBalancesAsync(string walletAddress, BlockchainIntegratedTypes blockchainIntegratedType)
    {
        var strategy = ResolveStrategy(blockchainIntegratedType);
        return strategy.GetBalancesAsync(walletAddress);
    }

    public Task<SimpleObjectResult> TransferTokenFundsAsync(BlockchainTokenTransferDTO blockchainTokenTransferDTO, BlockchainIntegratedTypes blockchainIntegratedType)
    {
        var strategy = ResolveStrategy(blockchainIntegratedType);
        return strategy.TransferTokenFundsAsync(blockchainTokenTransferDTO);
    }

    public Task<SimpleObjectResult> TestIntegrationUpAsync(BlockchainIntegratedTypes blockchainIntegratedType)
    {
        var strategy = ResolveStrategy(blockchainIntegratedType);
        return strategy.TestIntegrationUpAsync();
    }

    private IBlockchainIntegrationStrategy ResolveStrategy(BlockchainIntegratedTypes blockchainIntegratedType)
    {
        var blockchainIntegrationStrategy = _strategies.FirstOrDefault(x => x.BlockchainIntegratedType == blockchainIntegratedType);
        if (blockchainIntegrationStrategy == null)
            throw new InvalidOperationException($"Invalid IntegrationType, invalidType={nameof(blockchainIntegratedType)} on BlockchainIntegrationStrategyContext");

        return blockchainIntegrationStrategy;
    }
}
