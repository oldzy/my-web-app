namespace Core.Models;

public class Cart
{
    public Guid Id { get; set; }
    public decimal TotalPrice => CartItems.Sum(item => item.Quantity * item.UnitPrice);
    public IEnumerable<CartItem> CartItems { get; set; } = [];
}
