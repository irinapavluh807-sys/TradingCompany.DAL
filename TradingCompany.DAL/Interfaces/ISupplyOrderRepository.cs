using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Interfaces;

public interface ISupplyOrderRepository : ICrudRepository<SupplyOrder>
{
    IReadOnlyList<SupplyOrder> GetActiveOrders();
}
