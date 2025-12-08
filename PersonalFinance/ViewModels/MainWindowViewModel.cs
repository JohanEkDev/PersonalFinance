using Microsoft.Extensions.DependencyInjection;
using PersonalFinance.Command;
using PersonalFinance.Models;
using PersonalFinance.Services;
using PersonalFinance.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PersonalFinance.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _services;

        private UserControl? _currentView;
        public UserControl? CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; RaisePropertyChanged(); }
        }

        public RelayCommand ShowSummaryCommand { get; }
        public RelayCommand ShowIncomeCommand { get; }
        public RelayCommand ShowExpenseCommand { get; }
        public RelayCommand ShowCategoriesCommand { get; }

        public MainWindowViewModel(IServiceProvider services)
        {
            _services = services;

            //Initialize commands.
            ShowSummaryCommand = new RelayCommand(_ => Navigate<SummaryView>());
            ShowIncomeCommand = new RelayCommand(_ => Navigate<IncomeView>());
            ShowExpenseCommand = new RelayCommand(_ => Navigate<ExpenseView>());
            ShowCategoriesCommand = new RelayCommand(_ => Navigate<CategoriesView>());

            //Dfault view.
            Navigate<SummaryView>();
        }

        private void Navigate<T>() where T : UserControl
        {
            CurrentView = _services.GetRequiredService<T>();
        }
    }
}
