using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Data;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

namespace TradingCompany.DAL.Tests;

public abstract class RepositoryTestBase : IDisposable
{
    private readonly string _databaseName;
    private readonly string _masterConnectionString;

    protected RepositoryTestBase()
    {
        _databaseName = $"TradingCompanyDalTests_{Guid.NewGuid():N}";
        var baseConnectionString = Environment.GetEnvironmentVariable("TRADING_COMPANY_TEST_SQLSERVER")
            ?? "Server=(localdb)\\MSSQLLocalDB;Integrated Security=True;TrustServerCertificate=True;";

        var databaseBuilder = new SqlConnectionStringBuilder(baseConnectionString)
        {
            InitialCatalog = _databaseName,
            Pooling = false,
            TrustServerCertificate = true
        };

        var masterBuilder = new SqlConnectionStringBuilder(databaseBuilder.ConnectionString)
        {
            InitialCatalog = "master"
        };

        _masterConnectionString = masterBuilder.ConnectionString;
        ConnectionFactory = new SqlServerConnectionFactory(databaseBuilder.ConnectionString);
        new DatabaseInitializer(ConnectionFactory).Initialize();
    }

    protected SqlServerConnectionFactory ConnectionFactory { get; }

    public void Dispose()
    {
        SqlConnection.ClearAllPools();

        using var connection = new SqlConnection(_masterConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"""
            IF DB_ID(@databaseName) IS NOT NULL
            BEGIN
                ALTER DATABASE {QuoteIdentifier(_databaseName)} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                DROP DATABASE {QuoteIdentifier(_databaseName)};
            END;
            """;
        command.Parameters.AddWithValue("@databaseName", _databaseName);
        command.ExecuteNonQuery();
    }

    private static string QuoteIdentifier(string value)
    {
        return "[" + value.Replace("]", "]]", StringComparison.Ordinal) + "]";
    }

    protected static string NewSuffix()
    {
        return Guid.NewGuid().ToString("N")[..8];
    }

    protected int CreateTestManager(string suffix)
    {
        return new WarehouseManagerRepository(ConnectionFactory).Create(new WarehouseManager
        {
            Username = $"test_manager_{suffix}",
            PasswordHash = "test-password-hash",
            FullName = "Test Manager",
            Email = $"test_manager_{suffix}@trading.local",
            CreatedAt = DateTime.UtcNow
        });
    }

    protected int CreateTestSupplier(string suffix)
    {
        return new SupplierRepository(ConnectionFactory).Create(new Supplier
        {
            Name = $"Test Supplier {suffix}",
            ContactName = "Test Contact",
            Phone = "+380500000000",
            Email = $"test_supplier_{suffix}@trading.local",
            Address = "Test address",
            IsActive = true
        });
    }

    protected int CreateTestProduct(int supplierId, string suffix)
    {
        return new ProductRepository(ConnectionFactory).Create(new Product
        {
            SupplierId = supplierId,
            Sku = $"TEST-{suffix}",
            Name = "Test Product",
            Description = "Product for repository test",
            UnitPrice = 25.50m,
            UnitOfMeasure = "piece",
            MinimumStockLevel = 3
        });
    }

    protected int CreateTestOrder(int supplierId, int managerId)
    {
        return new SupplyOrderRepository(ConnectionFactory).Create(new SupplyOrder
        {
            SupplierId = supplierId,
            ManagerId = managerId,
            Status = "Draft",
            OrderDate = DateTime.UtcNow,
            ExpectedDeliveryDate = DateTime.UtcNow.AddDays(3),
            Notes = "Test order"
        });
    }
}
