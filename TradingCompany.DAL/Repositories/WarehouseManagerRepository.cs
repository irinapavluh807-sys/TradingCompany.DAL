using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Data;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DAL.Models;

namespace TradingCompany.DAL.Repositories;

public sealed class WarehouseManagerRepository : SqlServerRepository, IWarehouseManagerRepository
{
    public WarehouseManagerRepository(SqlServerConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public IReadOnlyList<WarehouseManager> GetAll()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, username, password_hash, full_name, email, created_at
            FROM warehouse_managers
            ORDER BY id;
            """;

        using var reader = command.ExecuteReader();
        var managers = new List<WarehouseManager>();
        while (reader.Read())
        {
            managers.Add(Map(reader));
        }

        return managers;
    }

    public WarehouseManager? GetById(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, username, password_hash, full_name, email, created_at
            FROM warehouse_managers
            WHERE id = @id;
            """;
        AddParameter(command, "@id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public WarehouseManager? GetByUsername(string username)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, username, password_hash, full_name, email, created_at
            FROM warehouse_managers
            WHERE username = @username;
            """;
        AddParameter(command, "@username", username);

        using var reader = command.ExecuteReader();
        return reader.Read() ? Map(reader) : null;
    }

    public int Create(WarehouseManager entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO warehouse_managers (username, password_hash, full_name, email, created_at)
            VALUES (@username, @password_hash, @full_name, @email, @created_at);
            """;
        AddParameters(command, entity);

        return ExecuteInsert(command);
    }

    public bool Update(WarehouseManager entity)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE warehouse_managers
            SET username = @username,
                password_hash = @password_hash,
                full_name = @full_name,
                email = @email,
                created_at = @created_at
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
        command.CommandText = "DELETE FROM warehouse_managers WHERE id = @id;";
        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static void AddParameters(SqlCommand command, WarehouseManager entity)
    {
        AddParameter(command, "@username", entity.Username);
        AddParameter(command, "@password_hash", entity.PasswordHash);
        AddParameter(command, "@full_name", entity.FullName);
        AddParameter(command, "@email", entity.Email);
        AddParameter(command, "@created_at", FormatDateTime(entity.CreatedAt));
    }

    private static WarehouseManager Map(SqlDataReader reader)
    {
        return new WarehouseManager
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            Username = reader.GetString(reader.GetOrdinal("username")),
            PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
            FullName = reader.GetString(reader.GetOrdinal("full_name")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            CreatedAt = ReadDateTime(reader, "created_at")
        };
    }
}
