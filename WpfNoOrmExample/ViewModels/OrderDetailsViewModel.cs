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
    public OrderItemListItemViewModel(long id, string title)
    {
        Id = id;
        Title = title;
    }

    public long Id { get; }
    
    public string Title { get; }
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
        
        var orderItemVms = orderDetails.Items.Select(x => new OrderItemListItemViewModel(x.Id, x.Title));
        
        foreach (var orderItemVm in orderItemVms)
        {
            OrderItems.Add(orderItemVm);
        }
    }
}
