using StockManager.Services;

namespace StockManager.Pages;

public partial class StoragePage : ContentPage
{
    private readonly IStockService _stockService;

    public StoragePage(IStockService stockService)
    {
        InitializeComponent();
        _stockService = stockService;
        BindingContext = this;
    }
}