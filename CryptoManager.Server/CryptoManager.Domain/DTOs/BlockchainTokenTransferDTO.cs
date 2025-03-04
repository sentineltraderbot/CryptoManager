using System;

namespace CryptoManager.Domain.DTOs;

public class BlockchainTokenTransferDTO
{
    public string SenderWalletAddress { get; set; }
    public string SenderPrivateKey { get; set; }
    public string ReceiverWalletAddress { get; set; }
    public decimal Amount { get; set; }
    public string Symbol { get; set; }
}
