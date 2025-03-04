using System;

namespace CryptoManager.Domain.DTOs;

public class TickerPriceDTO
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
}
