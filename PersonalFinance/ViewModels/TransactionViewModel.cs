using PersonalFinance.Command;
using PersonalFinance.Models;
using PersonalFinance.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinance.ViewModels
{
    public class TransactionViewModel : BaseViewModel
    {
        private ObservableCollection<FinancialTransaction> _transactions;
        public ObservableCollection<FinancialTransaction> Transactions
        {
            get { return _transactions; }
            set { _transactions = value; }
        }

        private FinancialTransaction _selectedTransaction;
        public FinancialTransaction SelectedTransaction
        {
            get { return _selectedTransaction; }
            set
            {
                _selectedTransaction = value;
                RaisePropertyChanged();
            }
        }

        private FinancialTransaction _newTransaction;

        public FinancialTransaction NewTransaction
        {
            get { return _newTransaction; }
            set
            {
                _newTransaction = value;
                RaisePropertyChanged();
            }
        }

        private readonly ITransactionService _transactionService;

        // Har en preferens att använda Interfacet istället för implementationen när jag typar dom här
        // RelayCommand skulle fungera lika bra.
        public ICommand AddTransactionCommand { get; }
        public ICommand RemoveTransactionCommand { get; }

        public TransactionViewModel(ITransactionService transactionService)
        {
            Transactions = new ObservableCollection<FinancialTransaction>();
            NewTransaction = new FinancialTransaction();

            //AddTransactionCommand = new RelayCommand(param => AddStudent(), param => CanAddTransaction());
            RemoveTransactionCommand = new RelayCommand(param => RemoveTransaction(), param => CanRemoveTransaction());
            _transactionService = transactionService;
        }

        private void AddStudent()
        {
            Transactions.Add(NewTransaction);
            NewTransaction = new FinancialTransaction(); // Reset NewStudent for next entry
            SelectedTransaction = Transactions.Last();
        }

        private void RemoveTransaction()
        {
            Transactions.Remove(SelectedTransaction);
        }

        //private bool CanAddTransaction()
        //{
        //    // Example validation: Ensure that necessary fields are filled
        //    return !string.IsNullOrWhiteSpace(NewTransaction.Name);
        //}

        private bool CanRemoveTransaction()
        {
            return SelectedTransaction != null;
        }

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal async Task LoadAsync()
        {
            var allTransactions = await _transactionService.GetAllTransactionsAsync();

            foreach (var transaction in allTransactions)
            {
                Transactions.Add(transaction);
            }
        }
    }
}
