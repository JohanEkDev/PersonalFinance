using PersonalFinance.Command;
using PersonalFinance.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {

        private readonly ISummaryService _summaryService;

        private int _totalIncome;
        public int TotalIncome
        {
            get => _totalIncome;
            set
            {
                if (_totalIncome == value) return;
                _totalIncome = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(NetTotal));
            }
        }

        private int _totalExpense;
        public int TotalExpense
        {
            get => _totalExpense;
            set
            {
                if (_totalExpense == value) return;
                _totalExpense = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(NetTotal));
            }
        }

        public int NetTotal => TotalIncome - TotalExpense;

        public RelayCommand ReloadCommand { get; }

        public SummaryViewModel(ISummaryService summaryService)
        {
            _summaryService = summaryService;

            ReloadCommand = new RelayCommand(async _ => await LoadAsync());
        }

        public async Task LoadAsync()
        {
            TotalIncome = await _summaryService.GetTotalIncomeAsync();
            TotalExpense = await _summaryService.GetTotalExpenseAsync();
        }
    }
}
