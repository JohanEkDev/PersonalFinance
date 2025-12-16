using PersonalFinance.Command;
using PersonalFinance.DTOs;
using PersonalFinance.Enums;
using PersonalFinance.Models;
using PersonalFinance.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Globalization;

    public class PrognosisViewModel : BaseViewModel
    {
        private readonly IPrognosisService _prognosisService;

        // Transactions for UI
        public ObservableCollection<FinancialTransaction> IncomeTransactions { get; } = new();
        public ObservableCollection<FinancialTransaction> ExpenseTransactions { get; } = new();

        // Month/Year selectors
        public ObservableCollection<string> Months { get; } = new();
        public ObservableCollection<int> Years { get; } = new();

        private int _selectedYear = DateTime.Today.Year;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear == value) return;
                _selectedYear = value;
                RaisePropertyChanged();
                RebuildMonths(); // update months for the newly selected year
            }
        }

        private int _selectedMonthIndex;
        public int SelectedMonthIndex
        {
            get => _selectedMonthIndex;
            set
            {
                if (_selectedMonthIndex == value) return;
                _selectedMonthIndex = value;
                RaisePropertyChanged();
            }
        }

        // Forecast totals
        private int _monthlyTotalIncome;
        public int MonthlyTotalIncome
        {
            get => _monthlyTotalIncome;
            private set
            {
                _monthlyTotalIncome = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(MonthlyTotalNet));
            }
        }

        private int _monthlyTotalExpense;
        public int MonthlyTotalExpense
        {
            get => _monthlyTotalExpense;
            private set
            {
                _monthlyTotalExpense = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(MonthlyTotalNet));
            }
        }

        public int MonthlyTotalNet => MonthlyTotalIncome - MonthlyTotalExpense;

        private int _yearlyTotalIncome;
        public int YearlyTotalIncome
        {
            get => _yearlyTotalIncome;
            private set {
                _yearlyTotalIncome = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(YearlyTotalNet));
            }
        }

        private int _yearlyTotalExpense;
        public int YearlyTotalExpense
        {
            get => _yearlyTotalExpense;
            private set
            {
                _yearlyTotalExpense = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(YearlyTotalNet));
            }
        }

        public int YearlyTotalNet => YearlyTotalIncome - YearlyTotalExpense;

        // Command to load prognosis
        public RelayCommand LoadCommand { get; }

        // Compute selected month date safely
        private DateTime SelectedMonthDate
        {
            get
            {
                int startMonth = SelectedYear == DateTime.Today.Year
                    ? DateTime.Today.Month + 1
                    : 1;

                int month = startMonth + SelectedMonthIndex;

                // clamp month to 12
                if (month > 12) month = 1;

                return new DateTime(SelectedYear, month, 1);
            }
        }

        public PrognosisViewModel(IPrognosisService prognosisService)
        {
            _prognosisService = prognosisService ?? throw new ArgumentNullException(nameof(prognosisService));
            LoadCommand = new RelayCommand(async _ => await LoadAsync());

            RebuildYears();
            RebuildMonths();
        }

        // Load prognosis from service
        public async Task LoadAsync()
        {
            var month = SelectedMonthDate;

            IncomeTransactions.Clear();
            ExpenseTransactions.Clear();

            // Monthly
            var monthly = await _prognosisService.GetMonthlyPrognosisAsync(month);

            foreach (var t in monthly.IncomeTransactions)
                IncomeTransactions.Add(t);

            foreach (var t in monthly.ExpenseTransactions)
                ExpenseTransactions.Add(t);

            MonthlyTotalIncome = monthly.TotalIncome;
            MonthlyTotalExpense = monthly.TotalExpense;

            // Yearly
            var yearly = await _prognosisService.GetYearlyPrognosisAsync(SelectedYear);
            YearlyTotalIncome = yearly.TotalIncome;
            YearlyTotalExpense = yearly.TotalExpense;
        }

        // Populate Years ComboBox with current + next 5 years
        private void RebuildYears()
        {
            Years.Clear();
            int currentYear = DateTime.Today.Year;
            int currentMonth = DateTime.Today.Month;
            int maxYear = currentYear + 5;

            for (int y = currentYear; y <= maxYear; y++)
            {
                if (y == currentYear && currentMonth == 12)
                    continue;

                Years.Add(y);
            }

            if (!Years.Contains(SelectedYear))
                SelectedYear = Years.FirstOrDefault();
        }

        // Populate Months ComboBox based on SelectedYear
        private void RebuildMonths()
        {
            Months.Clear();

            var allMonths = CultureInfo
                .GetCultureInfo("en-US")
                .DateTimeFormat
                .MonthNames
                .ToArray();

            int startMonth = SelectedYear == DateTime.Today.Year
                ? DateTime.Today.Month + 1
                : 1;

            if (startMonth > 12)
                startMonth = 12;

            for (int m = startMonth; m <= 12; m++)
                Months.Add(allMonths[m - 1]);

            SelectedMonthIndex = 0;
        }
    }

}
