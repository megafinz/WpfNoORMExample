namespace WpfNoOrmExample.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(OrderListViewModel orderListViewModel)
    {
        OrderListViewModel = orderListViewModel;
    }

    public OrderListViewModel OrderListViewModel { get; }
}
