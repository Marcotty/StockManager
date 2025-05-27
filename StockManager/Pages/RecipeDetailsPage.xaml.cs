using StockManager.Model;

namespace StockManager.Pages;

public partial class RecipeDetailsPage : ContentPage
{
	public Recipe LocalRecipe { get; set; }

    public RecipeDetailsPage(Recipe recipe)
	{
        InitializeComponent();
        LocalRecipe = recipe;
        BindingContext = this;
    }
    protected override void OnAppearing()
    {
        OnPropertyChanged(nameof(LocalRecipe));
    }
}