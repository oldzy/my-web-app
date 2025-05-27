using System;

namespace Api.Models;

public class CartItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
}
