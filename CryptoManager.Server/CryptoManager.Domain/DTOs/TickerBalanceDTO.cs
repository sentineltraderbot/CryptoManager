namespace CryptoManager.Domain.DTOs;

public class TickerBalanceDTO
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public decimal? Balance { get; set; }
    public string PublicKey { get; set; }
}
