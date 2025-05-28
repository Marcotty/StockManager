using StockManager.Model;
using StockManager.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StockManager.Pages;

public partial class RecipesPage : ContentPage
{
	private readonly IStockService _stockService;
    private List<Recipe> _recipes;
    public ObservableCollection<Recipe> FilteredRecipes { get; set; }
    public ICommand AddRecipeToCartCommand => new Command<Recipe>(OnAddToCartClicked);
    public ICommand ItemTappedCommand => new Command<Recipe>(OnItemTapped);

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

    private async void OnItemTapped(Recipe recipe)
    {
        if (recipe == null) return;
        var itemDetailsPage = new RecipeDetailsPage(_stockService, recipe);
        await Navigation.PushAsync(itemDetailsPage);
    }

    private async void OnAddToCartClicked(Recipe recipe)
    {
        if (recipe == null) return;
        foreach (var ingredient in recipe.Ingredients)
        {
            _stockService.UpdateItemToShoppingList(ingredient);
        }
        bool confirm = await DisplayAlert("Success", $"{recipe.Name} ingredients has been added to your shopping list.\\n Go to shopping list ?", "Yes", "No");
        if (confirm)
        {
            await Shell.Current.GoToAsync("//ShoppingPage");
        }
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