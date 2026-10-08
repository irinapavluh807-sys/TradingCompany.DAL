using System.Globalization;
using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Data;

namespace TradingCompany.DAL.Repositories;

public abstract class SqlServerRepository
{
    private readonly SqlServerConnectionFactory _connectionFactory;

    protected SqlServerRepository(SqlServerConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    protected SqlConnection OpenConnection()
    {
        var connection = _connectionFactory.CreateConnection();
        connection.Open();

        return connection;
    }

    protected static void AddParameter(SqlCommand command, string name, object? value)
    {
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);
    }

    protected static int ExecuteInsert(SqlCommand command)
    {
        command.CommandText = command.CommandText.TrimEnd().TrimEnd(';') + "; SELECT CAST(SCOPE_IDENTITY() AS int);";

        return Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture);
    }

    protected static DateTime FormatDateTime(DateTime value)
    {
        return value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value.ToUniversalTime();
    }

    protected static DateTime ReadDateTime(SqlDataReader reader, string columnName)
    {
        return reader.GetDateTime(reader.GetOrdinal(columnName));
    }

    protected static DateTime? ReadNullableDateTime(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
        {
            return null;
        }

        return reader.GetDateTime(ordinal);
    }

    protected static decimal ReadDecimal(SqlDataReader reader, string columnName)
    {
        return Convert.ToDecimal(reader[columnName], CultureInfo.InvariantCulture);
    }
}
