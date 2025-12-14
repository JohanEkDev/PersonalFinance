using PersonalFinance.Command;
using PersonalFinance.Enums;
using PersonalFinance.Models;
using PersonalFinance.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PersonalFinance.ViewModels
{
    public class ExpenseViewModel : BaseViewModel
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;

        public ObservableCollection<FinancialTransaction> Transactions { get; } = new();
        public ObservableCollection<Category> AllCategories { get; } = new();

        private FinancialTransaction? _selectedTransaction;
        public FinancialTransaction? SelectedTransaction
        {
            get { return _selectedTransaction; }
            set { _selectedTransaction = value; RaisePropertyChanged(); PopulateEditFields(); }
        }

        // Editing / add fields (simple approach)
        public int Amount { get; set; }
        public FrequencyOfTransaction Frequency { get; set; } = FrequencyOfTransaction.OneTime;
        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged(); }
        }

        //Expose frequencies for ComboBox binding.
        public IEnumerable<FrequencyOfTransaction> Frequencies =>
            Enum.GetValues(typeof(FrequencyOfTransaction)).Cast<FrequencyOfTransaction>();

        //Commands.
        public RelayCommand AddCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ClearCommand { get; }

        public ExpenseViewModel(ITransactionService transactionService, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;

            AddCommand = new RelayCommand(async _ => await AddTransactionAsync(), _ => SelectedTransaction == null);
            SaveCommand = new RelayCommand(async _ => await SaveTransactionAsync(), _ => SelectedTransaction != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteTransactionAsync(), _ => SelectedTransaction != null);
            ClearCommand = RelayCommand.FromAction(ClearFields);
        }

        public async Task LoadAsync()
        {
            await LoadCategoriesAsync();
            await LoadTransactionsAsync();
        }

        public async Task LoadTransactionsAsync()
        {
            Transactions.Clear();
            var expenseTransactions = await _transactionService.GetAllExpenseTransactionsAsync();
            foreach (var t in expenseTransactions) Transactions.Add(t);
        }

        private async Task LoadCategoriesAsync()
        {
            AllCategories.Clear();
            var categories = await _categoryService.GetAllCategoriesAsync();
            foreach (var c in categories)
                AllCategories.Add(c);
        }

        private void PopulateEditFields()
        {
            if (SelectedTransaction != null)
            {
                Amount = SelectedTransaction.Amount;
                Frequency = SelectedTransaction.Frequency;
                SelectedCategory = AllCategories
                    .FirstOrDefault(c => c.Id == SelectedTransaction.Category?.Id);

                RaisePropertyChanged(nameof(Amount));
                RaisePropertyChanged(nameof(Frequency));
                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }

        public async Task AddTransactionAsync()
        {
            if (SelectedCategory == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            try
            {
                var newTransaction = new FinancialTransaction
                {
                    Amount = Amount,
                    Type = TypeOfTransaction.Expense,
                    Frequency = Frequency,
                    Category = SelectedCategory
                };

                await _transactionService.AddTransactionAsync(newTransaction);
                Transactions.Add(newTransaction);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add transaction: {ex.Message}");
            }
        }

        public async Task SaveTransactionAsync()
        {
            if (SelectedTransaction == null || SelectedCategory == null)
                return;

            try
            {
                SelectedTransaction.Amount = Amount;
                SelectedTransaction.Frequency = Frequency;
                SelectedTransaction.Category = SelectedCategory;

                await _transactionService.EditTransactionAsync(SelectedTransaction);
                // Refresh list item (simple approach: reload everything)
                await LoadTransactionsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save: {ex.Message}");
            }
        }

        public async Task DeleteTransactionAsync()
        {
            if (SelectedTransaction == null) return;

            try
            {
                await _transactionService.DeleteTransactionAsync(SelectedTransaction.Id);
                Transactions.Remove(SelectedTransaction);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete: {ex.Message}");
            }
        }

        private void ClearFields()
        {
            SelectedTransaction = null;
            Amount = 0;
            Frequency = FrequencyOfTransaction.OneTime;
            SelectedCategory = null;

            RaisePropertyChanged(nameof(Amount));
            RaisePropertyChanged(nameof(Frequency));
            RaisePropertyChanged(nameof(SelectedCategory));
        }
    }
}
