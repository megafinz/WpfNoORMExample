using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WpfNoOrmExample.Db;

public static class Migrator
{
    public static void RunMigrations(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetService<IMigrationRunner>();
        runner?.MigrateUp();
    }
}
