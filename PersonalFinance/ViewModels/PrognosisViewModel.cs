using PersonalFinance.Command;
using PersonalFinance.DTOs;
using PersonalFinance.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.ViewModels
{
    public class PrognosisViewModel : BaseViewModel
    {
        private readonly IPrognosisService _prognosisService;

        private DateTime _selectedMonth = DateTime.Today;
        public DateTime SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth == value) return;
                _selectedMonth = value;
                RaisePropertyChanged();
                LoadAsync(); //Reload data when month changes. How do we set this up better for async?
            }
        }

        public int ForecastIncome { get; private set; }
        public int ForecastExpense { get; private set; }
        public int ForecastNet => ForecastIncome - ForecastExpense;

        public ObservableCollection<ForecastItem> Incomes { get; } = new();
        public ObservableCollection<ForecastItem> Expenses { get; } = new();

        public PrognosisViewModel(IPrognosisService prognosisService)
        {
            _prognosisService = prognosisService;
        }

        public async Task LoadAsync()
        {
            var result = await _prognosisService.GetMonthlyForecastAsync(SelectedMonth);

            ForecastIncome = result.TotalIncome;
            ForecastExpense = result.TotalExpense;

            Incomes.Clear();
            Expenses.Clear();

            foreach (var item in result.Incomes)
                Incomes.Add(item);

            foreach (var item in result.Expenses)
                Expenses.Add(item);

            RaisePropertyChanged(nameof(ForecastIncome));
            RaisePropertyChanged(nameof(ForecastExpense));
            RaisePropertyChanged(nameof(ForecastNet));
        }
    }
}
