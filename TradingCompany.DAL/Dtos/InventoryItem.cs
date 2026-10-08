namespace TradingCompany.DAL.Dtos;

public sealed class InventoryItem
{
    public int ProductId { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public string SupplierName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public string UnitOfMeasure { get; set; } = string.Empty;

    public int MinimumStockLevel { get; set; }

    public int QuantityOnHand { get; set; }

    public int ReservedQuantity { get; set; }

    public int AvailableQuantity => QuantityOnHand - ReservedQuantity;

    public DateTime? UpdatedAt { get; set; }
}
