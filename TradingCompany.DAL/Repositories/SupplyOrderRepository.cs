using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Data;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Repositories;

public sealed class SupplyOrderRepository : SqlServerRepository, ISupplyOrderRepository
{
    public SupplyOrderRepository(SqlServerConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public IReadOnlyList<SupplyOrder> GetAll()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, supplier_id, manager_id, status, order_date, expected_delivery_date, notes
            FROM supply_orders
            ORDER BY id;
            """;

        using var reader = command.ExecuteReader();
        var orders = new List<SupplyOrder>();
        while (reader.Read())
        {
            orders.Add(Map(reader));
        }

        return orders;
    }

    public IReadOnlyList<SupplyOrder> GetActiveOrders()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, supplier_id, manager_id, status, order_date, expected_delivery_date, notes
            FROM supply_orders
            WHERE status IN ('Draft', 'Ordered', 'Shipped')
            ORDER BY expected_delivery_date, id;
            """;

        using var reader = command.ExecuteReader();
        var orders = new List<SupplyOrder>();
        while (reader.Read())
        {
            orders.Add(Map(reader));
        }

        return orders;
    }

    public SupplyOrder? GetById(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, supplier_id, manager_id, status, order_date, expected_delivery_date, notes
            FROM supply_orders
            WHERE id = @id;
            """;
        AddParameter(command, "@id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public int Create(SupplyOrder entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO supply_orders (supplier_id, manager_id, status, order_date, expected_delivery_date, notes)
            VALUES (@supplier_id, @manager_id, @status, @order_date, @expected_delivery_date, @notes);
            """;
        AddParameters(command, entity);

        return ExecuteInsert(command);
    }

    public bool Update(SupplyOrder entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE supply_orders
            SET supplier_id = @supplier_id,
                manager_id = @manager_id,
                status = @status,
                order_date = @order_date,
                expected_delivery_date = @expected_delivery_date,
                notes = @notes
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
        command.CommandText = "DELETE FROM supply_orders WHERE id = @id;";
        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static void AddParameters(SqlCommand command, SupplyOrder entity)
    {
        AddParameter(command, "@supplier_id", entity.SupplierId);
        AddParameter(command, "@manager_id", entity.ManagerId);
        AddParameter(command, "@status", entity.Status);
        AddParameter(command, "@order_date", FormatDateTime(entity.OrderDate));
        AddParameter(command, "@expected_delivery_date", entity.ExpectedDeliveryDate.HasValue
            ? FormatDateTime(entity.ExpectedDeliveryDate.Value)
            : null);
        AddParameter(command, "@notes", entity.Notes);
    }

    private static SupplyOrder Map(SqlDataReader reader)
    {
        return new SupplyOrder
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            SupplierId = reader.GetInt32(reader.GetOrdinal("supplier_id")),
            ManagerId = reader.GetInt32(reader.GetOrdinal("manager_id")),
            Status = reader.GetString(reader.GetOrdinal("status")),
            OrderDate = ReadDateTime(reader, "order_date"),
            ExpectedDeliveryDate = ReadNullableDateTime(reader, "expected_delivery_date"),
            Notes = reader.GetString(reader.GetOrdinal("notes"))
        };
    }
}
