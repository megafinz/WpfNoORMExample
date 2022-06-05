using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Windows;
using WpfNoOrmExample.ViewModels;

namespace WpfNoOrmExample.Views;

public partial class OrderListView
{
    public OrderListView()
    {
        InitializeComponent();
        
        DataContextChanged += OnDataContextChanged;

        this.WhenActivated(disposable =>
        {
            // Order List
            this.OneWayBind(
                ViewModel, 
                viewModel => viewModel.Orders, 
                view => view.OrdersListBox.ItemsSource)
                .DisposeWith(disposable);

            // Order List Visibility
            this.OneWayBind(
                ViewModel, 
                viewModel => viewModel.HasOrders, 
                view => view.OrdersPanel.Visibility,
                x => x ? Visibility.Visible : Visibility.Hidden)
                .DisposeWith(disposable);
            
            // "No Orders" Text Block Visibility
            this.OneWayBind(
                    ViewModel, 
                    viewModel => viewModel.HasOrders, 
                    view => view.NoOrdersTextBlock.Visibility,
                    x => x ? Visibility.Hidden : Visibility.Visible)
                .DisposeWith(disposable);

            // Selected Order
            this.Bind(
                ViewModel,
                viewModel => viewModel.SelectedOrderId,
                view => view.OrdersListBox.SelectedValue)
                .DisposeWith(disposable);
            
            // Order Details Visibility
            this.OneWayBind(
                ViewModel,
                viewModel => viewModel.SelectedOrderId,
                view => view.OrderDetailsPanel.Visibility,
                x => x is null ? Visibility.Hidden : Visibility.Visible)
                .DisposeWith(disposable);
            
            // Order Details
            this.OneWayBind(
                ViewModel,
                viewModel => viewModel.SelectedOrderDetails,
                view => view.OrderDetails.DataContext)
                .DisposeWith(disposable);
        });
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        ViewModel = DataContext as OrderListViewModel;
        ViewModel?.LoadOrders.Execute().Subscribe();
    }
}
