using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

namespace TradingCompany.DAL.Tests;

public sealed class WarehouseStockRepositoryTests : RepositoryTestBase
{
    [Fact]
    public void CreateReadUpdateDelete_Works()
    {
        var suppliers = new SupplierRepository(ConnectionFactory);
        var products = new ProductRepository(ConnectionFactory);
        var repository = new WarehouseStockRepository(ConnectionFactory);
        var suffix = NewSuffix();
        var supplierId = CreateTestSupplier(suffix);
        var productId = CreateTestProduct(supplierId, suffix);

        Assert.True(repository.GetAll().Count >= 20);

        var id = repository.Create(new WarehouseStock
        {
            ProductId = productId,
            QuantityOnHand = 25,
            ReservedQuantity = 2,
            UpdatedAt = DateTime.UtcNow
        });

        var created = repository.GetById(id);
        Assert.NotNull(created);
        Assert.Equal(productId, created.ProductId);
        Assert.NotNull(repository.GetByProductId(productId));

        created.QuantityOnHand = 35;
        created.ReservedQuantity = 4;
        Assert.True(repository.Update(created));

        var updated = repository.GetById(id)!;
        Assert.Equal(35, updated.QuantityOnHand);
        Assert.Equal(4, updated.ReservedQuantity);

        Assert.True(repository.Delete(id));
        Assert.Null(repository.GetById(id));
        Assert.True(products.Delete(productId));
        Assert.True(suppliers.Delete(supplierId));
    }
}
