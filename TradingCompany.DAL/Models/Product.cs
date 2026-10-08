namespace TradingCompany.DAL.Models;

public sealed class Product
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public string UnitOfMeasure { get; set; } = string.Empty;

    public int MinimumStockLevel { get; set; }
}
