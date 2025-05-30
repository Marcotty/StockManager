using StockManager.Model;
using StockManager.Services;
using System.Windows.Input;

namespace StockManager.Pages;

public partial class RecipeDetailsPage : ContentPage
{
    private readonly IStockService _stockService;
    public Recipe LocalRecipe { get; set; }
    private bool canCook = false;
    public ICommand AddRecipeToCartCommand => new Command<Recipe>(OnAddToCartClicked);
    public ICommand CookItCommand => new Command<Recipe>(OnCookItClicked);

    public RecipeDetailsPage(IStockService stockService, Recipe recipe)
    {
        InitializeComponent();
        _stockService = stockService;
        LocalRecipe = recipe;

        flexLayout.Children.Clear();
        foreach (var ingredient in LocalRecipe.Ingredients)
        {
            var label = new Label
            {
                Text = $"*   {ingredient.Name} ({ingredient.Quantity} {ingredient.QuantityUnit})",
                FontSize = 12,
                Margin = new Thickness(4, 2)
            };
            flexLayout.Children.Add(label);
        }

        canCook = _stockService.CanCookRecipe(LocalRecipe);
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var child = CookButton.Children.LastOrDefault();
        canCook = _stockService.CanCookRecipe(LocalRecipe);
        if (!canCook)
        {
            if (child is Label)
            {
                ((Label)child).TextColor = Color.Parse("Grey");
            }
        }

        OnPropertyChanged(nameof(LocalRecipe));
    }

    private async void OnCookItClicked(Recipe recipe)
    {
        if (recipe == null) return;
        if(!canCook)
        {
            await DisplayAlert("Error", "You don't have enough ingredients to cook this recipe.", "OK");
            return;
        }
        bool confirm = await DisplayAlert("Cook It", $"Do you want to cook {recipe.Name}? Ingredients will be taken from stock", "Yes", "No");
        if (confirm)
        {
            _stockService.RemoveIngredientsFromRecipe(recipe);
            await DisplayAlert("Cooking", $"You are now cooking {recipe.Name}!", "OK");
        }
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