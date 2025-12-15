using PersonalFinance.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Services
{
    public interface IPrognosisService
    {
        Task<MonthlyForecastResult> GetMonthlyForecastAsync(DateTime selectedMonth);
    }
}
