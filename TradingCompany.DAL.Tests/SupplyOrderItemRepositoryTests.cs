using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

namespace TradingCompany.DAL.Tests;

public sealed class SupplyOrderItemRepositoryTests : RepositoryTestBase
{
    [Fact]
    public void CreateReadUpdateDelete_Works()
    {
        var managers = new WarehouseManagerRepository(ConnectionFactory);
        var suppliers = new SupplierRepository(ConnectionFactory);
        var products = new ProductRepository(ConnectionFactory);
        var orders = new SupplyOrderRepository(ConnectionFactory);
        var repository = new SupplyOrderItemRepository(ConnectionFactory);
        var suffix = NewSuffix();
        var managerId = CreateTestManager(suffix);
        var supplierId = CreateTestSupplier(suffix);
        var productId = CreateTestProduct(supplierId, suffix);
        var orderId = CreateTestOrder(supplierId, managerId);

        Assert.True(repository.GetAll().Count >= 20);

        var id = repository.Create(new SupplyOrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            QuantityOrdered = 8,
            UnitPrice = 25.50m
        });

        var created = repository.GetById(id);
        Assert.NotNull(created);
        Assert.Equal(orderId, created.OrderId);
        Assert.Contains(repository.GetByOrderId(orderId), item => item.Id == id);

        created.QuantityOrdered = 12;
        created.UnitPrice = 27.75m;
        Assert.True(repository.Update(created));

        var updated = repository.GetById(id)!;
        Assert.Equal(12, updated.QuantityOrdered);
        Assert.Equal(27.75m, updated.UnitPrice);

        Assert.True(repository.Delete(id));
        Assert.Null(repository.GetById(id));
        Assert.True(orders.Delete(orderId));
        Assert.True(products.Delete(productId));
        Assert.True(suppliers.Delete(supplierId));
        Assert.True(managers.Delete(managerId));
    }
}
