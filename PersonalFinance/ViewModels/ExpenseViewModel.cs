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
    public class ExpenseViewModel : ViewModelBase
    {
        private readonly ITransactionService _transactionService;

        public ObservableCollection<FinancialTransaction> Transactions { get; } = new();

        private FinancialTransaction? _selectedTransaction;
        public FinancialTransaction? SelectedTransaction
        {
            get => _selectedTransaction;
            set { _selectedTransaction = value; RaisePropertyChanged(); PopulateEditFields(); }
        }

        // Editing / add fields (simple approach)
        public int Amount { get; set; }
        public FrequencyOfTransaction Frequency { get; set; } = FrequencyOfTransaction.OneTime;
        public string CategoryName { get; set; } = string.Empty;

        // Expose frequencies for ComboBox binding
        public IEnumerable<FrequencyOfTransaction> Frequencies =>
            Enum.GetValues(typeof(FrequencyOfTransaction)).Cast<FrequencyOfTransaction>();

        // Commands
        public RelayCommand LoadCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ClearCommand { get; }

        public ExpenseViewModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;

            LoadCommand = new RelayCommand(async _ => await LoadTransactionsAsync());
            AddCommand = new RelayCommand(async _ => await AddTransactionAsync());
            SaveCommand = new RelayCommand(async _ => await SaveTransactionAsync(), _ => SelectedTransaction != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteTransactionAsync(), _ => SelectedTransaction != null);
            ClearCommand = RelayCommand.FromAction(ClearFields);

            // load initial data
            _ = LoadTransactionsAsync();
        }

        private void PopulateEditFields()
        {
            if (SelectedTransaction != null)
            {
                Amount = SelectedTransaction.Amount;
                Frequency = SelectedTransaction.Frequency;
                CategoryName = SelectedTransaction.Category?.Name ?? string.Empty;

                RaisePropertyChanged(nameof(Amount));
                RaisePropertyChanged(nameof(Frequency));
                RaisePropertyChanged(nameof(CategoryName));
            }
        }

        public async Task LoadTransactionsAsync()
        {
            Transactions.Clear();
            var items = await _transactionService.GetAllExpenseTransactionsAsync();
            foreach (var t in items) Transactions.Add(t);
        }

        public async Task AddTransactionAsync()
        {
            try
            {
                var tx = new FinancialTransaction
                {
                    Amount = Amount,
                    Type = TypeOfTransaction.Expense,
                    Frequency = Frequency,
                    Category = new Category { Name = CategoryName }
                };

                await _transactionService.AddTransactionAsync(tx);
                Transactions.Add(tx);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add transaction: {ex.Message}");
            }
        }

        public async Task SaveTransactionAsync()
        {
            if (SelectedTransaction == null) return;
            try
            {
                SelectedTransaction.Amount = Amount;
                SelectedTransaction.Frequency = Frequency;
                SelectedTransaction.Category = new Category { Name = CategoryName };

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
            CategoryName = string.Empty;

            RaisePropertyChanged(nameof(Amount));
            RaisePropertyChanged(nameof(Frequency));
            RaisePropertyChanged(nameof(CategoryName));
        }
    }
}
