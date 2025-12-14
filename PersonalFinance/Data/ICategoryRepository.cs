using PersonalFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Data
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task AddAsync(Category category);
        Task EditAsync(Category category);
        Task DeleteAsync(int id);
    }
}
