using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Interfaces;

public interface IWarehouseManagerRepository : ICrudRepository<WarehouseManager>
{
    WarehouseManager? GetByUsername(string username);
}
