using System;

namespace CryptoManager.Domain.DTOs;

public class ApplicationUserDTO
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string ImageURL { get; set; }
    public bool HasReceivedAirdrop { get; set; }
    public string SolanaWalletAddress { get; set; }
    public string ReferredById { get; set; }
}
