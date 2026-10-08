using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

namespace TradingCompany.DAL.Tests;

public sealed class SupplyOrderRepositoryTests : RepositoryTestBase
{
    [Fact]
    public void CreateReadUpdateDelete_Works()
    {
        var managers = new WarehouseManagerRepository(ConnectionFactory);
        var suppliers = new SupplierRepository(ConnectionFactory);
        var repository = new SupplyOrderRepository(ConnectionFactory);
        var suffix = NewSuffix();
        var managerId = CreateTestManager(suffix);
        var supplierId = CreateTestSupplier(suffix);

        Assert.True(repository.GetAll().Count >= 20);

        var id = repository.Create(new SupplyOrder
        {
            SupplierId = supplierId,
            ManagerId = managerId,
            Status = "Draft",
            OrderDate = DateTime.UtcNow,
            ExpectedDeliveryDate = DateTime.UtcNow.AddDays(7),
            Notes = "Repository order"
        });

        var created = repository.GetById(id);
        Assert.NotNull(created);
        Assert.Equal("Draft", created.Status);

        created.Status = "Ordered";
        created.Notes = "Updated repository order";
        Assert.True(repository.Update(created));

        var updated = repository.GetById(id)!;
        Assert.Equal("Ordered", updated.Status);
        Assert.Contains(repository.GetActiveOrders(), order => order.Id == id);

        Assert.True(repository.Delete(id));
        Assert.Null(repository.GetById(id));
        Assert.True(suppliers.Delete(supplierId));
        Assert.True(managers.Delete(managerId));
    }
}
