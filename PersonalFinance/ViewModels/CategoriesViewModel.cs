using PersonalFinance.Command;
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
    public class CategoriesViewModel : BaseViewModel
    {
        private readonly ICategoryService _categoryService;

        public ObservableCollection<Category> Categories { get; } = new();

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged();
                PopulateEditFields();
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set { _name = value; RaisePropertyChanged(); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ClearCommand { get; }

        public CategoriesViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            AddCommand = new RelayCommand(async _ => await AddCategoryAsync(), _ => SelectedCategory == null);
            SaveCommand = new RelayCommand(async _ => await SaveCategoryAsync(), _ => SelectedCategory != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteCategoryAsync(), _ => SelectedCategory != null);
            ClearCommand = RelayCommand.FromAction(ClearFields);
        }

        public async Task LoadAsync()
        {
            await LoadCategoriesAsync();
        }

        public async Task LoadCategoriesAsync()
        {
            Categories.Clear();
            var categories = await _categoryService.GetAllCategoriesAsync();
            foreach (var c in categories) Categories.Add(c);
        }

        private void PopulateEditFields()
        {
            if (SelectedCategory != null)
            {
                Name = SelectedCategory.Name;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public async Task AddCategoryAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Category name is required.");
                return;
            }

            try
            {
                var newCategory = new Category { Name = this.Name };
                await _categoryService.AddCategoryAsync(newCategory);
                Categories.Add(newCategory);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}");
            }
        }

        public async Task SaveCategoryAsync()
        {
            if (SelectedCategory == null) return;

            try
            {
                SelectedCategory.Name = Name;
                await _categoryService.EditCategoryAsync(SelectedCategory);
                await LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}");
            }
        }

        public async Task DeleteCategoryAsync()
        {
            if (SelectedCategory == null) return;

            try
            {
                await _categoryService.DeleteCategoryAsync(SelectedCategory.Id);
                Categories.Remove(SelectedCategory);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting: {ex.Message}");
            }
        }

        private void ClearFields()
        {
            SelectedCategory = null;
            Name = string.Empty;

            RaisePropertyChanged(nameof(Name));
        }
    }
}
