using System.Reflection;
using Microsoft.Data.SqlClient;

namespace TradingCompany.DAL.Data;

public sealed class DatabaseInitializer
{
    private readonly SqlServerConnectionFactory _connectionFactory;

    public DatabaseInitializer(SqlServerConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Initialize()
    {
        EnsureDatabaseExists();

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        ExecuteScript(connection, ReadEmbeddedSql("schema.sql"));
        ExecuteScript(connection, ReadEmbeddedSql("seed.sql"));
    }

    private void EnsureDatabaseExists()
    {
        var builder = new SqlConnectionStringBuilder(_connectionFactory.ConnectionString);
        var databaseName = builder.InitialCatalog;

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            return;
        }

        builder.InitialCatalog = "master";
        using var connection = new SqlConnection(builder.ConnectionString);
        connection.Open();

        if (DatabaseExists(connection, databaseName))
        {
            return;
        }

        if (IsLocalDb(builder.DataSource) && TryAttachExistingLocalDbFiles(connection, databaseName))
        {
            return;
        }

        CreateDatabase(connection, databaseName);
    }

    private static bool DatabaseExists(SqlConnection connection, string databaseName)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT CASE WHEN DB_ID(@databaseName) IS NULL THEN 0 ELSE 1 END;";
        command.Parameters.AddWithValue("@databaseName", databaseName);

        return Convert.ToInt32(command.ExecuteScalar()) == 1;
    }

    private static bool TryAttachExistingLocalDbFiles(SqlConnection connection, string databaseName)
    {
        var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var dataFilePath = Path.Combine(userProfilePath, $"{databaseName}.mdf");

        if (!File.Exists(dataFilePath))
        {
            return false;
        }

        var logFilePath = Path.Combine(userProfilePath, $"{databaseName}_log.ldf");
        var filesSql = File.Exists(logFilePath)
            ? $"(FILENAME = {QuoteString(dataFilePath)}), (FILENAME = {QuoteString(logFilePath)})"
            : $"(FILENAME = {QuoteString(dataFilePath)})";

        using var command = connection.CreateCommand();
        command.CommandText = File.Exists(logFilePath)
            ? $"CREATE DATABASE {QuoteIdentifier(databaseName)} ON {filesSql} FOR ATTACH;"
            : $"CREATE DATABASE {QuoteIdentifier(databaseName)} ON {filesSql} FOR ATTACH_REBUILD_LOG;";
        command.ExecuteNonQuery();

        return true;
    }

    private static void CreateDatabase(SqlConnection connection, string databaseName)
    {
        using var command = connection.CreateCommand();
        command.CommandText = """
            IF DB_ID(@databaseName) IS NULL
            BEGIN
                DECLARE @createDatabaseSql nvarchar(max) = N'CREATE DATABASE ' + QUOTENAME(@databaseName);
                EXEC sys.sp_executesql @createDatabaseSql;
            END;
            """;
        command.Parameters.AddWithValue("@databaseName", databaseName);
        command.ExecuteNonQuery();
    }

    private static bool IsLocalDb(string dataSource)
    {
        return dataSource.Contains("(localdb)", StringComparison.OrdinalIgnoreCase);
    }

    private static string QuoteIdentifier(string value)
    {
        return "[" + value.Replace("]", "]]", StringComparison.Ordinal) + "]";
    }

    private static string QuoteString(string value)
    {
        return "N'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
    }

    private static void ExecuteScript(SqlConnection connection, string script)
    {
        using var command = connection.CreateCommand();
        command.CommandText = script;
        command.CommandTimeout = 60;
        command.ExecuteNonQuery();
    }

    private static string ReadEmbeddedSql(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly
            .GetManifestResourceNames()
            .SingleOrDefault(name => name.EndsWith($".{fileName}", StringComparison.OrdinalIgnoreCase));

        if (resourceName is null)
        {
            throw new InvalidOperationException($"Embedded SQL resource '{fileName}' was not found.");
        }

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded SQL resource '{resourceName}' cannot be opened.");
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
