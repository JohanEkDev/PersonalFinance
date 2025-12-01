using PersonalFinance.Command;
using PersonalFinance.Models;
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
    public class TransactionViewModel : ViewModelBase
    {
        public ObservableCollection<Transaction> Transactions { get; set; }
        private Transaction _selectedTransaction;

        public Transaction SelectedTransaction
        {
            get { return _selectedTransaction; }
            set
            {
                _selectedTransaction = value;
                RaisePropertyChanged();
            }
        }

        private Transaction _newTransaction;

        public Transaction NewTransaction
        {
            get { return _newTransaction; }
            set
            {
                _newTransaction = value;
                RaisePropertyChanged();
            }
        }


        // Har en preferens att använda Interfacet istället för implementationen när jag typar dom här
        // RelayCommand skulle fungera lika bra.
        public ICommand AddTransactionCommand { get; }
        public ICommand RemoveTransactionCommand { get; }

        public TransactionViewModel()
        {
            Transactions = new ObservableCollection<Transaction>();
            NewTransaction = new Transaction();

            AddTransactionCommand = new RelayCommand(param => AddStudent(), param => CanAddTransaction());
            RemoveTransactionCommand = new RelayCommand(param => RemoveTransaction(), param => CanRemoveTransaction());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void AddStudent()
        {
            Transactions.Add(NewTransaction);
            NewTransaction = new Transaction(); // Reset NewStudent for next entry
            SelectedTransaction = Transactions.Last();
        }

        private void RemoveTransaction()
        {
            Transactions.Remove(SelectedTransaction);
        }

        private bool CanAddTransaction()
        {
            // Example validation: Ensure that necessary fields are filled
            return !string.IsNullOrWhiteSpace(NewTransaction.Name);
        }

        private bool CanRemoveTransaction()
        {
            return SelectedTransaction != null;
        }

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
