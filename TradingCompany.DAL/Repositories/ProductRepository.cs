using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Data;
using TradingCompany.DAL.Dtos;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Repositories;

public sealed class ProductRepository : SqlServerRepository, IProductRepository
{
    public ProductRepository(SqlServerConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public IReadOnlyList<Product> GetAll()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, supplier_id, sku, name, description, unit_price, unit_of_measure, minimum_stock_level
            FROM products
            ORDER BY id;
            """;

        using var reader = command.ExecuteReader();
        var products = new List<Product>();
        while (reader.Read())
        {
            products.Add(Map(reader));
        }

        return products;
    }

    public Product? GetById(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, supplier_id, sku, name, description, unit_price, unit_of_measure, minimum_stock_level
            FROM products
            WHERE id = @id;
            """;
        AddParameter(command, "@id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public IReadOnlyList<InventoryItem> SearchInventory(
        string? searchTerm,
        InventorySortField sortBy = InventorySortField.Name,
        bool descending = false)
    {
        var orderBy = sortBy switch
        {
            InventorySortField.Sku => "p.sku",
            InventorySortField.SupplierName => "supplier_name",
            InventorySortField.QuantityOnHand => "quantity_on_hand",
            InventorySortField.UnitPrice => "p.unit_price",
            _ => "p.name"
        };
        var direction = descending ? "DESC" : "ASC";

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            SELECT p.id AS product_id,
                   p.sku,
                   p.name AS product_name,
                   s.name AS supplier_name,
                   p.unit_price,
                   p.unit_of_measure,
                   p.minimum_stock_level,
                   COALESCE(ws.quantity_on_hand, 0) AS quantity_on_hand,
                   COALESCE(ws.reserved_quantity, 0) AS reserved_quantity,
                   ws.updated_at
            FROM products p
            INNER JOIN suppliers s ON s.id = p.supplier_id
            LEFT JOIN warehouse_stocks ws ON ws.product_id = p.id
            WHERE (@search IS NULL
                   OR LOWER(p.sku) LIKE '%' + LOWER(@search) + '%'
                   OR LOWER(p.name) LIKE '%' + LOWER(@search) + '%'
                   OR LOWER(s.name) LIKE '%' + LOWER(@search) + '%')
            ORDER BY {orderBy} {direction}, p.id ASC;
            """;
        AddParameter(command, "@search", string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim());

        using var reader = command.ExecuteReader();
        var items = new List<InventoryItem>();
        while (reader.Read())
        {
            items.Add(new InventoryItem
            {
                ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                Sku = reader.GetString(reader.GetOrdinal("sku")),
                ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                SupplierName = reader.GetString(reader.GetOrdinal("supplier_name")),
                UnitPrice = ReadDecimal(reader, "unit_price"),
                UnitOfMeasure = reader.GetString(reader.GetOrdinal("unit_of_measure")),
                MinimumStockLevel = reader.GetInt32(reader.GetOrdinal("minimum_stock_level")),
                QuantityOnHand = reader.GetInt32(reader.GetOrdinal("quantity_on_hand")),
                ReservedQuantity = reader.GetInt32(reader.GetOrdinal("reserved_quantity")),
                UpdatedAt = ReadNullableDateTime(reader, "updated_at")
            });
        }

        return items;
    }

    public int Create(Product entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO products (supplier_id, sku, name, description, unit_price, unit_of_measure, minimum_stock_level)
            VALUES (@supplier_id, @sku, @name, @description, @unit_price, @unit_of_measure, @minimum_stock_level);
            """;
        AddParameters(command, entity);

        return ExecuteInsert(command);
    }

    public bool Update(Product entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE products
            SET supplier_id = @supplier_id,
                sku = @sku,
                name = @name,
                description = @description,
                unit_price = @unit_price,
                unit_of_measure = @unit_of_measure,
                minimum_stock_level = @minimum_stock_level
            WHERE id = @id;
            """;
        AddParameter(command, "@id", entity.Id);
        AddParameters(command, entity);

        return command.ExecuteNonQuery() > 0;
    }

    public bool Delete(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM products WHERE id = @id;";
        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static void AddParameters(SqlCommand command, Product entity)
    {
        AddParameter(command, "@supplier_id", entity.SupplierId);
        AddParameter(command, "@sku", entity.Sku);
        AddParameter(command, "@name", entity.Name);
        AddParameter(command, "@description", entity.Description);
        AddParameter(command, "@unit_price", entity.UnitPrice);
        AddParameter(command, "@unit_of_measure", entity.UnitOfMeasure);
        AddParameter(command, "@minimum_stock_level", entity.MinimumStockLevel);
    }

    private static Product Map(SqlDataReader reader)
    {
        return new Product
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            SupplierId = reader.GetInt32(reader.GetOrdinal("supplier_id")),
            Sku = reader.GetString(reader.GetOrdinal("sku")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Description = reader.GetString(reader.GetOrdinal("description")),
            UnitPrice = ReadDecimal(reader, "unit_price"),
            UnitOfMeasure = reader.GetString(reader.GetOrdinal("unit_of_measure")),
            MinimumStockLevel = reader.GetInt32(reader.GetOrdinal("minimum_stock_level"))
        };
    }
}
