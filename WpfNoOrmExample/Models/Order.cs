namespace WpfNoOrmExample.Models;

public record Order(long Id, string Title);
public record OrderDetails(long Id, string Title, OrderItem[] Items);
public record OrderItem(long Id, string Title, int Quantity, decimal PriceAmount, string PriceCurrency);
