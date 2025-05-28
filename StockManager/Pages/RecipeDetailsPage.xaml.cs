using StockManager.Model;
using StockManager.Services;
using System.Windows.Input;

namespace StockManager.Pages;

public partial class RecipeDetailsPage : ContentPage
{
    private readonly IStockService _stockService;
    public Recipe LocalRecipe { get; set; }
    public ICommand AddRecipeToCartCommand => new Command<Recipe>(OnAddToCartClicked);

    public RecipeDetailsPage(IStockService stockService, Recipe recipe)
	{
        InitializeComponent();
        _stockService = stockService;
        LocalRecipe = recipe;
        BindingContext = this;
    }
    protected override void OnAppearing()
    {
        OnPropertyChanged(nameof(LocalRecipe));
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
}