using Npgsql;
using System;
using System.Data;

namespace WpfNoOrmExample.Db;

public interface IDbConnectionProvider
{
    IDbConnection GetDbConnection();
}

public sealed class NpgsqlConnectionProvider : IDbConnectionProvider
{
    private readonly string _dbConnectionString;

    public NpgsqlConnectionProvider()
    {
        var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        if (dbConnectionString is not { Length: > 0 })
        {
            throw new InvalidOperationException("Missing connection string (env var DB_CONNECTION_STRING)");
        }

        _dbConnectionString = dbConnectionString;
    }
    
    public IDbConnection GetDbConnection() => new NpgsqlConnection(_dbConnectionString);
}
