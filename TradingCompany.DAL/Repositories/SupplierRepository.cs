using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Data;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Repositories;

public sealed class SupplierRepository : SqlServerRepository, ISupplierRepository
{
    public SupplierRepository(SqlServerConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public IReadOnlyList<Supplier> GetAll()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, name, contact_name, phone, email, address, is_active
            FROM suppliers
            ORDER BY id;
            """;

        using var reader = command.ExecuteReader();
        var suppliers = new List<Supplier>();
        while (reader.Read())
        {
            suppliers.Add(Map(reader));
        }

        return suppliers;
    }

    public Supplier? GetById(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, name, contact_name, phone, email, address, is_active
            FROM suppliers
            WHERE id = @id;
            """;
        AddParameter(command, "@id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public int Create(Supplier entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO suppliers (name, contact_name, phone, email, address, is_active)
            VALUES (@name, @contact_name, @phone, @email, @address, @is_active);
            """;
        AddParameters(command, entity);

        return ExecuteInsert(command);
    }

    public bool Update(Supplier entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE suppliers
            SET name = @name,
                contact_name = @contact_name,
                phone = @phone,
                email = @email,
                address = @address,
                is_active = @is_active
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
        command.CommandText = "DELETE FROM suppliers WHERE id = @id;";
        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static void AddParameters(SqlCommand command, Supplier entity)
    {
        AddParameter(command, "@name", entity.Name);
        AddParameter(command, "@contact_name", entity.ContactName);
        AddParameter(command, "@phone", entity.Phone);
        AddParameter(command, "@email", entity.Email);
        AddParameter(command, "@address", entity.Address);
        AddParameter(command, "@is_active", entity.IsActive);
    }

    private static Supplier Map(SqlDataReader reader)
    {
        return new Supplier
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            ContactName = reader.GetString(reader.GetOrdinal("contact_name")),
            Phone = reader.GetString(reader.GetOrdinal("phone")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            Address = reader.GetString(reader.GetOrdinal("address")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"))
        };
    }
}
