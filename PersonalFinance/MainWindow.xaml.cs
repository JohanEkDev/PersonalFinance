using PersonalFinance.Services;
using PersonalFinance.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalFinance
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            //Loaded += MainWindow_Loaded;
        }

        //private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    await _viewModel.LoadAsync();
        //}
    }
}