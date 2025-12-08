using PersonalFinance.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Models
{
    public class FinancialTransaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public TypeOfTransaction Type { get; set; }
        public FrequencyOfTransaction Frequency {  get; set; }
        public Category Category { get; set; } = new();
    }
}
