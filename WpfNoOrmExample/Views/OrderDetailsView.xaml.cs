using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Windows;
using WpfNoOrmExample.ViewModels;

namespace WpfNoOrmExample.Views;

public partial class OrderDetailsView
{
    public OrderDetailsView()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;

        this.WhenActivated(disposable =>
        {
            // Id
            this.OneWayBind(
                ViewModel,
                viewModel => viewModel.Id,
                view => view.IdTextBlock.Text)
                .DisposeWith(disposable);
            
            // Title
            this.OneWayBind(
                ViewModel,
                viewModel => viewModel.Title,
                view => view.TitleTextBlock.Text)
                .DisposeWith(disposable);
            
            // Order Items List
            this.OneWayBind(
                ViewModel,
                viewModel => viewModel.OrderItems,
                view => view.OrderItemsDataGrid.ItemsSource)
                .DisposeWith(disposable);

            // Order Items List Visibility
            this.OneWayBind(
                ViewModel,
                viewModel => viewModel.OrderItems.Count,
                view => view.OrderItemsList.Visibility,
                x => x > 0 ? Visibility.Visible : Visibility.Collapsed)
                .DisposeWith(disposable);
            
            // No Order Items Text Block Visibility
            this.OneWayBind(
                ViewModel,
                viewModel => viewModel.OrderItems.Count,
                view => view.NoOrderItemsTextBlock.Visibility,
                x => x > 0 ? Visibility.Collapsed : Visibility.Visible)
                .DisposeWith(disposable);
        });
    }

    private void OnDataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        ViewModel = DataContext as OrderDetailsViewModel;
        ViewModel?.LoadOrderDetails.Execute().Subscribe();
    }
}
