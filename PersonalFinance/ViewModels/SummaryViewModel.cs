using PersonalFinance.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.ViewModels
{
    public class SummaryViewModel : ViewModelBase
    {

        public string Title => "Summary";

        // Example properties you will later populate via services
        public ObservableCollection<string> SummaryLines { get; } = new();

        public SummaryViewModel()
        {
            // demo data
            SummaryLines.Add("Total Income: 0");
            SummaryLines.Add("Total Expense: 0");
        }
    }
}
