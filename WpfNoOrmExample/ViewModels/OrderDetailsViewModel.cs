using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using WpfNoOrmExample.Services;

namespace WpfNoOrmExample.ViewModels;

public sealed class OrderItemListItemViewModel : ViewModelBase
{
    public OrderItemListItemViewModel(long id, string title, int quantity, decimal priceAmount, string priceCurrency)
    {
        Id = id;
        Title = title;
        Quantity = quantity;
        PriceAmount = priceAmount;
        PriceCurrency = priceCurrency;
    }
    
    public long Id { get; }
    
    public string Title { get; }
    
    public int Quantity { get; }
    
    public decimal PriceAmount { get; }
    
    public string PriceCurrency { get; }
}

public sealed class OrderDetailsViewModel : ViewModelBase
{
    private readonly IOrderRepo _orderRepo;

    public OrderDetailsViewModel(IOrderRepo orderRepo, long id)
    {
        _orderRepo = orderRepo;
        Id = id;
        LoadOrderDetails = ReactiveCommand.CreateFromTask(DoLoadOrderDetails);
    }

    public long Id { get; }
    
    [Reactive]
    public string? Title { get; private set; }

    public ObservableCollection<OrderItemListItemViewModel> OrderItems { get; } = new();
    
    public ReactiveCommand<Unit, Unit> LoadOrderDetails { get; }

    private async Task DoLoadOrderDetails()
    {
        var orderDetails = await _orderRepo.GetOrderById(Id);

        Title = orderDetails.Title;
        
        var orderItemVms = orderDetails.Items.Select(x => 
            new OrderItemListItemViewModel(
                x.Id, 
                x.Title,
                x.Quantity,
                x.PriceAmount,
                x.PriceCurrency
            )
        );
        
        foreach (var orderItemVm in orderItemVms)
        {
            OrderItems.Add(orderItemVm);
        }
    }
}
