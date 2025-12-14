using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalFinance.Data;
using PersonalFinance.Services;
using PersonalFinance.ViewModels;
using PersonalFinance.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PersonalFinance
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext,services) =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    //Add Repositories.
                    services.AddTransient<ITransactionRepository, TransactionRepository>();
                    services.AddTransient<ICategoryRepository, CategoryRepository>();

                    //Add Service.
                    services.AddTransient<ITransactionService, TransactionService>();
                    services.AddTransient<ICategoryService, CategoryService>();
                    services.AddTransient<ISummaryService, SummaryService>();

                    //Add ViewModels.
                    services.AddTransient<SummaryViewModel>();
                    services.AddTransient<IncomeViewModel>();
                    services.AddTransient<ExpenseViewModel>();
                    services.AddTransient<CategoriesViewModel>();

                    //Add Views.
                    services.AddTransient<SummaryView>();
                    services.AddTransient<IncomeView>();
                    services.AddTransient<ExpenseView>();
                    services.AddTransient<CategoriesView>();

                    //Add Singletons.
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainWindowViewModel>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            if (AppHost != null)
            {
                await AppHost.StartAsync();
                var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (AppHost != null)
            {
                await AppHost.StopAsync();
                AppHost.Dispose();
            }
            base.OnExit(e);
        }
    }

}
