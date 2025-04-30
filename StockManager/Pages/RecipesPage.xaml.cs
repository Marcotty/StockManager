using StockManager.Services;

namespace StockManager.Pages;

public partial class RecipesPage : ContentPage
{
	private readonly IStockService _stockService;
    public RecipesPage(IStockService stockService)
    {
        InitializeComponent();
        _stockService = stockService;
        BindingContext = this;
    }
}