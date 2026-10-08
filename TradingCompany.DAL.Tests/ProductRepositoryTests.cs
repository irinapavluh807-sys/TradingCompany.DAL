using TradingCompany.DAL.Dtos;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

namespace TradingCompany.DAL.Tests;

public sealed class ProductRepositoryTests : RepositoryTestBase
{
    [Fact]
    public void CreateReadUpdateDelete_Works()
    {
        var suppliers = new SupplierRepository(ConnectionFactory);
        var repository = new ProductRepository(ConnectionFactory);
        var suffix = NewSuffix();
        var supplierId = CreateTestSupplier(suffix);

        Assert.True(repository.GetAll().Count >= 20);

        var id = repository.Create(new Product
        {
            SupplierId = supplierId,
            Sku = $"PRODUCT-{suffix}",
            Name = "Repository Product",
            Description = "Repository test product",
            UnitPrice = 44.40m,
            UnitOfMeasure = "piece",
            MinimumStockLevel = 9
        });

        var created = repository.GetById(id);
        Assert.NotNull(created);
        Assert.Equal($"PRODUCT-{suffix}", created.Sku);

        created.UnitPrice = 55.50m;
        created.MinimumStockLevel = 11;
        Assert.True(repository.Update(created));

        var updated = repository.GetById(id)!;
        Assert.Equal(55.50m, updated.UnitPrice);
        Assert.Equal(11, updated.MinimumStockLevel);

        var inventoryMatches = repository.SearchInventory("Repository Product", InventorySortField.Name);
        Assert.Contains(inventoryMatches, item => item.ProductId == id);

        Assert.True(repository.Delete(id));
        Assert.Null(repository.GetById(id));
        Assert.True(suppliers.Delete(supplierId));
    }
}
