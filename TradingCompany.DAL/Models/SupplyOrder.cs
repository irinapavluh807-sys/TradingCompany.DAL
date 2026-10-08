namespace TradingCompany.DAL.Models;

public sealed class SupplyOrder
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public int ManagerId { get; set; }

    public string Status { get; set; } = "Draft";

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpectedDeliveryDate { get; set; }

    public string Notes { get; set; } = string.Empty;
}
