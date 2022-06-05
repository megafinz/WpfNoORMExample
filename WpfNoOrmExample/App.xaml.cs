using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfNoOrmExample.Db;
using WpfNoOrmExample.ViewModels;
using WpfNoOrmExample.Views;

namespace WpfNoOrmExample;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var serviceProvider = new Bootstrapper().Run();

        InitDb();
        InitMainWindow();

        void InitDb()
        {
            // You may want to run this as a separate process/app since DB management shouldn't be really a concern of
            // the GUI app
            Migrator.RunMigrations(serviceProvider);
        }

        void InitMainWindow()
        {
            var mainWindow = new MainWindow
            {
                DataContext = serviceProvider.GetService<MainWindowViewModel>()
            };
        
            Current.MainWindow = mainWindow;
        
            mainWindow.Show();
        }
    }
    
}
