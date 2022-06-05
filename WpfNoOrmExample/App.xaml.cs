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
