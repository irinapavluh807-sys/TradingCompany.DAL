using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Interfaces;

public interface ISupplyOrderItemRepository : ICrudRepository<SupplyOrderItem>
{
    IReadOnlyList<SupplyOrderItem> GetByOrderId(int orderId);
}
