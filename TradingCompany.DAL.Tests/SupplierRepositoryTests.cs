using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

namespace TradingCompany.DAL.Tests;

public sealed class SupplierRepositoryTests : RepositoryTestBase
{
    [Fact]
    public void CreateReadUpdateDelete_Works()
    {
        var repository = new SupplierRepository(ConnectionFactory);
        var suffix = NewSuffix();

        Assert.True(repository.GetAll().Count >= 20);

        var id = repository.Create(new Supplier
        {
            Name = $"Repository Supplier {suffix}",
            ContactName = "Repository Contact",
            Phone = "+380501234567",
            Email = $"supplier_test_{suffix}@trading.local",
            Address = "1 Test Street",
            IsActive = true
        });

        var created = repository.GetById(id);
        Assert.NotNull(created);
        Assert.Equal($"Repository Supplier {suffix}", created.Name);

        created.Phone = "+380509999999";
        created.IsActive = false;
        Assert.True(repository.Update(created));

        var updated = repository.GetById(id)!;
        Assert.Equal("+380509999999", updated.Phone);
        Assert.False(updated.IsActive);

        Assert.True(repository.Delete(id));
        Assert.Null(repository.GetById(id));
    }
}
