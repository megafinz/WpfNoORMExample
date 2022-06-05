using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WpfNoOrmExample.Services;

namespace WpfNoOrmExample.ViewModels;

public delegate OrderDetailsViewModel OrderDetailsViewModelFactory(long id);

public sealed class OrderListItemViewModel : ViewModelBase
{
    public OrderListItemViewModel(long id, string title)
    {
        Id = id;
        Title = title;
    }

    public long Id { get; }
    
    public string Title { get; }
}

public sealed class OrderListViewModel : ViewModelBase
{
    private readonly IOrderRepo _orderRepo;

    public OrderListViewModel(IOrderRepo orderRepo, OrderDetailsViewModelFactory orderDetailsFactory)
    {
        _orderRepo = orderRepo;
        
        LoadOrders = ReactiveCommand.CreateFromTask(DoLoadOrders);

        this.WhenAnyValue(x => x.SelectedOrderId)
            .Where(x => x is not null)
            .Select(x => x!.Value)
            .Select(x => orderDetailsFactory(x))
            .ToPropertyEx(this, x => x.SelectedOrderDetails);
    }
    
    public ReactiveCommand<Unit, Unit> LoadOrders { get; }

    public ObservableCollection<OrderListItemViewModel> Orders { get; } = new();
    
    [Reactive]
    public bool HasOrders { get; private set; }
    
    [Reactive]
    public long? SelectedOrderId { get; private set; }

    [ObservableAsProperty]
    public OrderDetailsViewModel? SelectedOrderDetails { get; private set; }

    private async Task DoLoadOrders()
    {
        var orders = await _orderRepo.GetOrders();
        var orderVms = orders.Select(x => new OrderListItemViewModel(x.Id, x.Title));

        foreach (var orderVm in orderVms)
        {
            Orders.Add(orderVm);
        }

        HasOrders = Orders.Count > 0;
    }
}
