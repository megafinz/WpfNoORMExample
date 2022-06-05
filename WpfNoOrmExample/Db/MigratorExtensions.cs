using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WpfNoOrmExample.Db;

public static class MigratorExtensions
{
    public static IServiceCollection ConfigureMigrations(this IServiceCollection services)
    {
        var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        if (dbConnectionString is not { Length: > 0 })
        {
            throw new InvalidOperationException("Missing connection string (env var DB_CONNECTION_STRING)");
        }

        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(dbConnectionString)
                .ScanIn(typeof(MigratorExtensions).Assembly).For.Migrations())
            .AddLogging(logging => logging.AddFluentMigratorConsole());

        return services;
    }
}
