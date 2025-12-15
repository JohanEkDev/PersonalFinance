using PersonalFinance.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.DTOs
{
    public class ForecastItem
    {
        public string Category { get; set; } = string.Empty;
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public FrequencyOfTransaction Frequency { get; set; }
    }
}
