using FluentMigrator;

namespace RedditNoOrmExample.Db.Migrations;

[Migration(2022_06_04_001)]
public class CreateTables : Migration 
{
    public override void Up()
    {
        Create.Table("Order")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Title").AsString().NotNullable();

        Insert.IntoTable("Order")
            .Row(new { Id = 1, Title = "Order 1" })
            .Row(new { Id = 2, Title = "Order 1" });

        Create.Table("OrderItem")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("OrderId").AsInt64().ForeignKey("Order", "Id")
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("PriceAmount").AsDecimal().NotNullable()
            .WithColumn("PriceCurrency").AsString().NotNullable();
        
        Insert.IntoTable("OrderItem")
            .Row(new { Id = 1, OrderId = 1, Title = "Order Item 1", Quantity = 10, PriceAmount = 10.00d, PriceCurrency = "USD" })
            .Row(new { Id = 2, OrderId = 1, Title = "Order Item 2", Quantity = 20, PriceAmount = 20.00d, PriceCurrency = "USD" });
    }

    public override void Down()
    {
        Delete.Table("Order");
        Delete.Table("OrderItem");
    }
}
