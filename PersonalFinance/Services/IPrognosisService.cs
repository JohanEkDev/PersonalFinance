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
        Task<MonthlyResult> GetMonthlyPrognosisAsync(DateTime selectedMonth);
        Task<YearlyResult> GetYearlyPrognosisAsync(int year);
    }
}
