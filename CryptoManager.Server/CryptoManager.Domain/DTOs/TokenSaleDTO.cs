using System;
using CryptoManager.Domain.Entities;

namespace CryptoManager.Domain.DTOs;

public class TokenSaleDTO
{
    public Guid? ApplicationUserId { get; set; }
    public OrderType OrderType { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
    public string UserWalletAddress { get; set; }
    public string BlockchainTx { get; set; }
    public decimal SOLQuantity { get; set; }
    public decimal SENTBOTQuantity { get; set; }
    public decimal Quantity { get; set; }
    public string ReferralWalletAddress { get; set; }
    public Guid? ReferralUserId { get; set; }
}
