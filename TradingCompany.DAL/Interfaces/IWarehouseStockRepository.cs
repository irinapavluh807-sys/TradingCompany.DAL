using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Interfaces;

public interface IWarehouseStockRepository : ICrudRepository<WarehouseStock>
{
    WarehouseStock? GetByProductId(int productId);
}
