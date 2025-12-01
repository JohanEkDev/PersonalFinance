using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinance.Command
{
    public class RelayCommand : ICommand
    {
        // Här sparar vi dom metoder som ska köras.

        // Action<object> är en delegering som refererar till en metod som tar en parameter av typen object.
        // Metoder är ju i sig inte objekt i sig, men när vi använder delegeringar som Action<object> kan vi
        // behandla metoder som om dom vore objekt. En delegering är alltså en referenstyp som kan peka på en metod.
        private readonly Action<object> _execute;

        // Func<object, bool> detta är också en delegering som refererar till en metod som tar en parametera v typen object
        // samt returnerar bool. Hade vi haft flera parametrar kunde vi skriva Func<object, object, bool> o.s.v.
        private readonly Func<object, bool> _canExecute;

        // Explicit deklaration av CanExecuteChanged-händelsen
        //public event EventHandler? CanExecuteChanged;
        // Metod för att utlösa CanExecuteChanged-händelsen
        //public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);


        // Konstruktor som tar emot execute och canExecute delegater och tildelar dom till våra fields ovan.
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // CommandManager.RequerySuggested kan vi använda för att automatiskt uppdatera när WPF-systemet känner
        // att något kan ha ändrat kommandots möjligheter att exekveras. Ex en UI-interaktion.
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Avgör om ett kommando kan exekveras i sitt nuvarande tillstånd
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        // Definierar metoden som ska kallas när kommandot ska exekveras.
        public void Execute(object parameter) => _execute(parameter);
    }
}
