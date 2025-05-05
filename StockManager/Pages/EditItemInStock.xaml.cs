using StockManager.Model;
using StockManager.Services;

namespace StockManager.Pages;

public partial class EditItemInStock : ContentPage
{
	private readonly IStockService _stockService;
    public Item Item { get; set; }
    public EditItemInStock(IStockService stockService, Item item)
	{
		InitializeComponent();
        _stockService = stockService;
        Item = item;
        BindingContext = this;
    }

    private void OnSaveChangesClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
        {
            DisplayAlert("Error", "Please enter a name for the item.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(Item.Location))
        {
            DisplayAlert("Error", "Please enter a location for the item.", "OK");
            return;
        }
        _stockService.UpdateItemToStockList(Item);
        Navigation.PopAsync();
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
        {
            DisplayAlert("Error", "Please enter a name for the item.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(Item.Location))
        {
            DisplayAlert("Error", "Please enter a location for the item.", "OK");
            return;
        }
        _stockService.UpdateItemToStockList(Item);
        Navigation.PopAsync();
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        _stockService.DeleteItemFromStock(Item.Id);
        Navigation.PopAsync();
    }
}