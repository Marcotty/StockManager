using StockManager.Model;
using StockManager.Services;
using System.Collections.ObjectModel;

namespace StockManager.Pages;

public partial class RecipesPage : ContentPage
{
	private readonly IStockService _stockService;
    private List<Recipe> _recipes;
    public ObservableCollection<Recipe> FilteredRecipes { get; set; }

    public RecipesPage(IStockService stockService)
    {
        InitializeComponent();
        _stockService = stockService;
        _recipes = _stockService.GetRecipes();
        FilteredRecipes = new ObservableCollection<Recipe>(_recipes);
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        FilteredRecipes.Clear();
        foreach (var item in _recipes)
        {
            FilteredRecipes.Add(item);
        }
        OnPropertyChanged(nameof(FilteredRecipes));
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        OnSearchQueryChanged(e.NewTextValue);
    }

    public void OnSearchQueryChanged(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            FilteredRecipes.Clear();
            foreach (var item in _recipes)
            {
                FilteredRecipes.Add(item);
            }
        }
        else
        {
            var filteredItems = _recipes
                .Where(item => item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                               item.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredRecipes.Clear();
            foreach (var item in filteredItems)
            {
                FilteredRecipes.Add(item);
            }
        }
        OnPropertyChanged(nameof(FilteredRecipes));
    }
}