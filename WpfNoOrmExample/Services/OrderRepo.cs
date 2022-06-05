using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WpfNoOrmExample.Db;
using WpfNoOrmExample.Models;

namespace WpfNoOrmExample.Services;

public interface IOrderRepo
{
    Task<Order[]> GetOrders();
    
    Task<OrderDetails> GetOrderById(long orderId);
}

// Not sure if SQL queries here are specific for Postgres, maybe you'll need a different implementation for a different DB
public sealed class OrderRepo :  IOrderRepo
{
    private readonly IDbConnectionProvider _dbConnectionProvider;

    public OrderRepo(IDbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }
    
    public async Task<Order[]> GetOrders()
    {
        using var connection = _dbConnectionProvider.GetDbConnection();
        var orders = await connection.QueryAsync<Order>(@"select * from ""Order""");
        return orders.ToArray();
    }

    public async Task<OrderDetails> GetOrderById(long orderId)
    {
        using var connection = _dbConnectionProvider.GetDbConnection();
        var orderDetails = await connection.QueryAsync(
            @"select 
                    o.""Id"" as OrderId, 
                    o.""Title"" as OrderTitle, 
                    oi.""Id"" as OrderItemId, 
                    oi.""Title"" as OrderItemTitle, 
                    oi.""Quantity"" as OrderItemQuantity, 
                    oi.""PriceAmount"" as OrderItemPriceAmount, 
                    oi.""PriceCurrency"" as OrderItemPriceCurrency 
                 from ""Order"" o
                 left join ""OrderItem"" oi
                 on oi.""OrderId"" = o.""Id""
                 where o.""Id""=@Id",
            new { Id = orderId }
        );
        
        var orderGrouping = orderDetails.GroupBy(x => new { x.orderid, x.ordertitle }).Single();
        
        return new OrderDetails(
            orderGrouping.Key.orderid, 
            orderGrouping.Key.ordertitle, 
            orderGrouping
                .Where(x => x.orderitemid is not null)
                .Select(x => 
                    new OrderItem(
                        x.orderitemid, 
                        x.orderitemtitle,
                        x.orderitemquantity,
                        x.orderitempriceamount,
                        x.orderitempricecurrency)).ToArray()
        );
    }
}

// You may want to use DynamicProxy from Castle.Core instead of plain decorators to reduce the boiler-plate
public sealed class LoggingOrderRepo : IOrderRepo
{
    private readonly IOrderRepo _impl;
    private readonly ILogger<LoggingOrderRepo> _logger;

    public LoggingOrderRepo(IOrderRepo impl, ILogger<LoggingOrderRepo> logger)
    {
        _impl = impl;
        _logger = logger;
    }

    public async Task<Order[]> GetOrders()
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation("Started getting Orders from DB...");
        var result = await _impl.GetOrders();
        sw.Stop();
        _logger.LogInformation("Finished getting Orders from DB in {ElapsedMs}ms", sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<OrderDetails> GetOrderById(long orderId)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation("Getting Order with id '{OrderId}' from DB...", orderId);
        var result = await _impl.GetOrderById(orderId);
        sw.Stop();
        _logger.LogInformation("Finished getting Order with id '{OrderId}' from DB in {ElapsedMs}ms", orderId, sw.ElapsedMilliseconds);
        return result;
    }
}
