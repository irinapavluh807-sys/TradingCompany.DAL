namespace TradingCompany.DAL.Models;

public sealed class SupplyOrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int QuantityOrdered { get; set; }

    public decimal UnitPrice { get; set; }
}
