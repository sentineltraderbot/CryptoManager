using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Blockchains;

namespace CryptoManager.Domain.Contracts.Integration;

public interface IBlockchainIntegrationStrategyContext
{
    Task<ObjectResult<IEnumerable<TickerBalanceDTO>>> GetBalancesAsync(string walletAddress, BlockchainIntegratedTypes blockchainIntegratedTypes);
    Task<SimpleObjectResult> TransferTokenFundsAsync(BlockchainTokenTransferDTO blockchainTransferDTO, BlockchainIntegratedTypes blockchainIntegratedTypes);
    Task<SimpleObjectResult> TestIntegrationUpAsync(BlockchainIntegratedTypes blockchainIntegratedTypes);
}
