using StockManager.Model;
using StockManager.Services;
using System.Collections.ObjectModel;

namespace StockManager.Pages;

public partial class ShoppingPage : ContentPage
{
    private readonly IStockService _stockService;
    public ObservableCollection<Item> Items { get; set; }
    public ShoppingPage(IStockService stockService)
    {
        InitializeComponent();
        _stockService = stockService;
        var shoppingPage = _stockService.GetDefaultItems();
        Items = new ObservableCollection<Item>(shoppingPage);
        BindingContext = this;
    }

    private void OnAddItem(object sender, EventArgs e)
    {
        Items.Add(new Item
        {
            Id = string.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Quantity = 0,
            Location = string.Empty,
            ExpirationDate = DateTime.Now,
            InStock = false
        });
        _stockService.AddNewItem();
        OnPropertyChanged(nameof(Items));
    }

    private void OnValidateClicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;
        var itemsToRemove = Items.Where(errand => errand.InStock).ToList();
        foreach (var errand in itemsToRemove)
        {
            Items.Remove(errand);
        }
        OnPropertyChanged(nameof(Items));
    }

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is not CheckBox checkBox)
            return;
        var stackLayout = checkBox.Parent as StackLayout;

        var entry = stackLayout?.Children.OfType<Entry>().FirstOrDefault();

        var item = entry?.BindingContext as Item;
        if (item != null)
        {
            item.InStock = checkBox.IsChecked;
            _stockService.UpdateItem(item);
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;
        var stackLayout = button.Parent as StackLayout;
        var entry = stackLayout?.Children.OfType<Entry>().FirstOrDefault();
        var item = entry?.BindingContext as Item;
        if (item != null)
        {
            Items.Remove(item);
            _stockService.DeleteItem(item.Id);
            OnPropertyChanged(nameof(Items));
        }
    }
}