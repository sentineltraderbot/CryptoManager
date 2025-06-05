using System;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoManager.Domain.Contracts.Entities;

namespace CryptoManager.Domain.Entities;

public class TokenSale : IEntity
{
    public Guid Id { get; set; }

    [ForeignKey("ApplicationUser")]
    public Guid? ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
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
    public virtual ApplicationUser ReferralUser { get; set; }
    public bool IsExcluded { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime RegistryDate { get; set; }
}
