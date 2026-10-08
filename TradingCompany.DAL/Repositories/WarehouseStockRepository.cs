using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Data;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Repositories;

public sealed class WarehouseStockRepository : SqlServerRepository, IWarehouseStockRepository
{
    public WarehouseStockRepository(SqlServerConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public IReadOnlyList<WarehouseStock> GetAll()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, product_id, quantity_on_hand, reserved_quantity, updated_at
            FROM warehouse_stocks
            ORDER BY id;
            """;

        using var reader = command.ExecuteReader();
        var stocks = new List<WarehouseStock>();
        while (reader.Read())
        {
            stocks.Add(Map(reader));
        }

        return stocks;
    }

    public WarehouseStock? GetById(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, product_id, quantity_on_hand, reserved_quantity, updated_at
            FROM warehouse_stocks
            WHERE id = @id;
            """;
        AddParameter(command, "@id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public WarehouseStock? GetByProductId(int productId)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, product_id, quantity_on_hand, reserved_quantity, updated_at
            FROM warehouse_stocks
            WHERE product_id = @product_id;
            """;
        AddParameter(command, "@product_id", productId);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public int Create(WarehouseStock entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO warehouse_stocks (product_id, quantity_on_hand, reserved_quantity, updated_at)
            VALUES (@product_id, @quantity_on_hand, @reserved_quantity, @updated_at);
            """;
        AddParameters(command, entity);

        return ExecuteInsert(command);
    }

    public bool Update(WarehouseStock entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE warehouse_stocks
            SET product_id = @product_id,
                quantity_on_hand = @quantity_on_hand,
                reserved_quantity = @reserved_quantity,
                updated_at = @updated_at
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
        command.CommandText = "DELETE FROM warehouse_stocks WHERE id = @id;";
        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static void AddParameters(SqlCommand command, WarehouseStock entity)
    {
        AddParameter(command, "@product_id", entity.ProductId);
        AddParameter(command, "@quantity_on_hand", entity.QuantityOnHand);
        AddParameter(command, "@reserved_quantity", entity.ReservedQuantity);
        AddParameter(command, "@updated_at", FormatDateTime(entity.UpdatedAt));
    }

    private static WarehouseStock Map(SqlDataReader reader)
    {
        return new WarehouseStock
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
            QuantityOnHand = reader.GetInt32(reader.GetOrdinal("quantity_on_hand")),
            ReservedQuantity = reader.GetInt32(reader.GetOrdinal("reserved_quantity")),
            UpdatedAt = ReadDateTime(reader, "updated_at")
        };
    }
}
