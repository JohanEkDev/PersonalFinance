using PersonalFinance.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalFinance.Views
{
    public partial class CategoriesView : UserControl
    {
        private readonly CategoriesViewModel _vm;

        public CategoriesView(CategoriesViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = vm;
            Loaded += CategoriesView_Loaded;
        }

        private async void CategoriesView_Loaded(object sender, RoutedEventArgs e)
        {
            await _vm.LoadAsync();
        }
    }
}
