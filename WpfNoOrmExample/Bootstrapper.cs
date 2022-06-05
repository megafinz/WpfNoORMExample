using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WpfNoOrmExample.Db;
using WpfNoOrmExample.Services;
using WpfNoOrmExample.ViewModels;

namespace WpfNoOrmExample;

public sealed class Bootstrapper
{
    public IServiceProvider Run()
    {
        var services = new ServiceCollection()
            .AddLogging(logger => logger.AddConsole())
            .ConfigureMigrations()
            .AddTransient<IDbConnectionProvider, NpgsqlConnectionProvider>()
            .AddTransient<OrderRepo>()
            .AddTransient<IOrderRepo>(serviceProvider => 
                new LoggingOrderRepo(
                    serviceProvider.GetRequiredService<OrderRepo>(),
                    serviceProvider.GetRequiredService<ILogger<LoggingOrderRepo>>()))
            .AddTransient<OrderListViewModel>()
            .AddTransient<OrderDetailsViewModelFactory>(serviceProvider => id => OrderDetailsFactory(serviceProvider, id))
            .AddTransient<MainWindowViewModel>();
        
        return services.BuildServiceProvider();

        OrderDetailsViewModel OrderDetailsFactory(IServiceProvider serviceProvider, long id) => 
            new(serviceProvider.GetRequiredService<IOrderRepo>(), id);
    }
}
