using StockManager.Model;

namespace StockManager.Pages;

public partial class RecipeDetailsPage : ContentPage
{
	private Recipe Recipe { get; set; }
    public RecipeDetailsPage(Recipe recipe)
	{
        Recipe = recipe;
        InitializeComponent();
	}
}