namespace Core.Models;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public uint Stock { get; set; }
    public string? Image { get; set; }
}
