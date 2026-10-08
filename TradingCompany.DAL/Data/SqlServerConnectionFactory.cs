using Microsoft.Data.SqlClient;

namespace TradingCompany.DAL.Data;

public sealed class SqlServerConnectionFactory
{
    public SqlServerConnectionFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}
