using TradingCompany.DAL.Dtos;
using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Interfaces;

public interface IProductRepository : ICrudRepository<Product>
{
    IReadOnlyList<InventoryItem> SearchInventory(
        string? searchTerm,
        InventorySortField sortBy = InventorySortField.Name,
        bool descending = false);
}
