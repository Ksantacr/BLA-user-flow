using System.Data;
using System.Data.Common;
using Npgsql;

namespace BLA.UserFlow.Infrastructure.DatabaseConnection;

public sealed class DbConnectionProvider
{
    private readonly string _connectionString;
    public DbConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public DbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}