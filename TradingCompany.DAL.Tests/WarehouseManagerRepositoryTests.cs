using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

namespace TradingCompany.DAL.Tests;

public sealed class WarehouseManagerRepositoryTests : RepositoryTestBase
{
    [Fact]
    public void CreateReadUpdateDelete_Works()
    {
        var repository = new WarehouseManagerRepository(ConnectionFactory);
        var suffix = NewSuffix();

        Assert.True(repository.GetAll().Count >= 20);

        var id = repository.Create(new WarehouseManager
        {
            Username = $"manager_test_{suffix}",
            PasswordHash = "test-password-hash",
            FullName = "Repository Manager",
            Email = $"manager_test_{suffix}@trading.local",
            CreatedAt = DateTime.UtcNow
        });

        var created = repository.GetById(id);
        Assert.NotNull(created);
        Assert.Equal($"manager_test_{suffix}", created.Username);
        Assert.NotNull(repository.GetByUsername(created.Username));

        created.FullName = "Updated Repository Manager";
        Assert.True(repository.Update(created));
        Assert.Equal("Updated Repository Manager", repository.GetById(id)!.FullName);

        Assert.True(repository.Delete(id));
        Assert.Null(repository.GetById(id));
    }
}
