using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Data;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Repositories;

public sealed class SupplyOrderItemRepository : SqlServerRepository, ISupplyOrderItemRepository
{
    public SupplyOrderItemRepository(SqlServerConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public IReadOnlyList<SupplyOrderItem> GetAll()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, order_id, product_id, quantity_ordered, unit_price
            FROM supply_order_items
            ORDER BY id;
            """;

        using var reader = command.ExecuteReader();
        var items = new List<SupplyOrderItem>();
        while (reader.Read())
        {
            items.Add(Map(reader));
        }

        return items;
    }

    public IReadOnlyList<SupplyOrderItem> GetByOrderId(int orderId)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, order_id, product_id, quantity_ordered, unit_price
            FROM supply_order_items
            WHERE order_id = @order_id
            ORDER BY id;
            """;
        AddParameter(command, "@order_id", orderId);

        using var reader = command.ExecuteReader();
        var items = new List<SupplyOrderItem>();
        while (reader.Read())
        {
            items.Add(Map(reader));
        }

        return items;
    }

    public SupplyOrderItem? GetById(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, order_id, product_id, quantity_ordered, unit_price
            FROM supply_order_items
            WHERE id = @id;
            """;
        AddParameter(command, "@id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public int Create(SupplyOrderItem entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO supply_order_items (order_id, product_id, quantity_ordered, unit_price)
            VALUES (@order_id, @product_id, @quantity_ordered, @unit_price);
            """;
        AddParameters(command, entity);

        return ExecuteInsert(command);
    }

    public bool Update(SupplyOrderItem entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE supply_order_items
            SET order_id = @order_id,
                product_id = @product_id,
                quantity_ordered = @quantity_ordered,
                unit_price = @unit_price
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
        command.CommandText = "DELETE FROM supply_order_items WHERE id = @id;";
        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static void AddParameters(SqlCommand command, SupplyOrderItem entity)
    {
        AddParameter(command, "@order_id", entity.OrderId);
        AddParameter(command, "@product_id", entity.ProductId);
        AddParameter(command, "@quantity_ordered", entity.QuantityOrdered);
        AddParameter(command, "@unit_price", entity.UnitPrice);
    }

    private static SupplyOrderItem Map(SqlDataReader reader)
    {
        return new SupplyOrderItem
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            OrderId = reader.GetInt32(reader.GetOrdinal("order_id")),
            ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
            QuantityOrdered = reader.GetInt32(reader.GetOrdinal("quantity_ordered")),
            UnitPrice = ReadDecimal(reader, "unit_price")
        };
    }
}
