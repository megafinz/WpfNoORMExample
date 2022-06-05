using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using WpfNoOrmExample.ViewModels;

namespace WpfNoOrmExample.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        DataContextChanged += OnDataContextChanged;

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(
                ViewModel, 
                viewModel => viewModel.OrderListViewModel, 
                view => view.OrderList.DataContext)
                .DisposeWith(disposable);
        });
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        ViewModel = DataContext as MainWindowViewModel;
    }
}
