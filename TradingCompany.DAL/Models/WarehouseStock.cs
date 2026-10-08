namespace TradingCompany.DAL.Models;

public sealed class WarehouseStock
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int QuantityOnHand { get; set; }

    public int ReservedQuantity { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
