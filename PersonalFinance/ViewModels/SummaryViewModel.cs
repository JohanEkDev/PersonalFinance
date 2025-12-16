using PersonalFinance.Command;
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
    public class SummaryViewModel : BaseViewModel
    {
        private readonly ISummaryService _summaryService;

        // ===== UI collections =====
        public ObservableCollection<FinancialTransaction> IncomeTransactions { get; } = new();
        public ObservableCollection<FinancialTransaction> ExpenseTransactions { get; } = new();

        public ObservableCollection<string> Months { get; } = new();
        public ObservableCollection<int> Years { get; } = new();

        // ===== Selected period =====
        private int _selectedYear = DateTime.Today.Year;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear == value) return;
                _selectedYear = value;
                RaisePropertyChanged();
                RebuildMonths();
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

        // ===== Totals =====
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
            private set
            {
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

        // ===== Commands =====
        public RelayCommand LoadCommand { get; }

        // ===== Derived selected date =====
        private DateTime SelectedMonthDate =>
            new DateTime(SelectedYear, SelectedMonthIndex + 1, 1);

        public SummaryViewModel(ISummaryService summaryService)
        {
            _summaryService = summaryService ?? throw new ArgumentNullException(nameof(summaryService));
            LoadCommand = new RelayCommand(async _ => await LoadAsync());

            BuildYears();
            RebuildMonths();
        }

        // ===== Load summary =====
        public async Task LoadAsync()
        {
            var month = SelectedMonthDate;

            IncomeTransactions.Clear();
            ExpenseTransactions.Clear();

            // Monthly
            var monthly = await _summaryService.GetMonthlySummaryAsync(month);

            foreach (var t in monthly.IncomeTransactions)
                IncomeTransactions.Add(t);

            foreach (var t in monthly.ExpenseTransactions)
                ExpenseTransactions.Add(t);

            MonthlyTotalIncome = monthly.TotalIncome;
            MonthlyTotalExpense = monthly.TotalExpense;

            // Yearly
            var yearly = await _summaryService.GetYearlySummaryAsync(SelectedYear);
            YearlyTotalIncome = yearly.TotalIncome;
            YearlyTotalExpense = yearly.TotalExpense;
        }

        // ===== Year setup (STATIC) =====
        private void BuildYears()
        {
            int currentYear = DateTime.Today.Year;

            // Show last 5 years + current
            for (int y = currentYear - 5; y <= currentYear; y++)
                Years.Add(y);

            SelectedYear = currentYear;
        }

        // ===== Month setup (dynamic) =====
        private void RebuildMonths()
        {
            Months.Clear();

            var allMonths = CultureInfo
                .GetCultureInfo("en-US")
                .DateTimeFormat
                .MonthNames
                .ToArray();

            int endMonth = SelectedYear == DateTime.Today.Year
                ? DateTime.Today.Month
                : 12;

            for (int m = 1; m <= endMonth; m++)
                Months.Add(allMonths[m - 1]);

            // Default to latest available month
            SelectedMonthIndex = Months.Count - 1;
        }
    }
}
