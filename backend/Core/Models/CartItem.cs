using System;

namespace Core.Models;

public class CartItem
{
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice {get; set; }
}
