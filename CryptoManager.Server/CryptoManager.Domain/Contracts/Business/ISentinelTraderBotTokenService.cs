using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoManager.Domain.DTOs;

namespace CryptoManager.Domain.Contracts.Business;

public interface ISentinelTraderBotTokenService
{
    Task<ObjectResult<IEnumerable<TickerBalanceDTO>>> GetBalancesAsync(string walletAddress);
    Task<SimpleObjectResult> SentinelTraderTokenAirdropAsync(string walletAddress, Guid userId);
}
