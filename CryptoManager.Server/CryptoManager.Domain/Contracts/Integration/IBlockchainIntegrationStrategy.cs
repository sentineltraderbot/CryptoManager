using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.IntegrationEntities.Blockchains;

namespace CryptoManager.Domain.Contracts.Integration;

public interface IBlockchainIntegrationStrategy
{
        BlockchainIntegratedTypes BlockchainIntegratedType { get; }
        Task<ObjectResult<IEnumerable<TickerBalanceDTO>>> GetBalancesAsync(string walletAddress);
        Task<SimpleObjectResult> TransferTokenFundsAsync(BlockchainTokenTransferDTO blockchainTokenTransferDTO);
        Task<SimpleObjectResult> TestIntegrationUpAsync();
}
