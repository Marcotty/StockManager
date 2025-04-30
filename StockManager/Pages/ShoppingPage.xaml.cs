using StockManager.Model;
using StockManager.Services;
using System.Collections.ObjectModel;

namespace StockManager.Pages;

public partial class ShoppingPage : ContentPage
{
    private readonly IStockService _stockService;
    public ObservableCollection<Item> Items { get; set; }
    public ObservableCollection<Item> DeletedItems { get; set; }
    private bool _isReverseEnabled;
    public bool IsReverseEnabled
    {
        get => _isReverseEnabled;
        set
        {
            _isReverseEnabled = value;
            OnPropertyChanged(nameof(IsReverseEnabled));
        }
    }

    public ShoppingPage(IStockService stockService)
    {
        InitializeComponent();
        _stockService = stockService;
        var shoppingPage = _stockService.GetDefaultItems();
        Items = [.. shoppingPage];
        DeletedItems = [];
        IsReverseEnabled = false;
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
        //((ListView)sender).ScrollTo(Items.Count - 1, position: ScrollToPosition.End, true);
        //((ListView)sender).Focus();
    }

    private void OnValidateClicked(object sender, EventArgs e)
    {
        var itemsToRemove = Items.Where(item => item.InStock).ToList();
        foreach (var item in itemsToRemove)
        {
            Items.Remove(item);
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
        var grid = button.Parent as Grid;
        var stackLayout = grid?.Children.OfType<StackLayout>().FirstOrDefault();
        var entry = stackLayout?.Children.OfType<Entry>().FirstOrDefault();
        var item = entry?.BindingContext as Item;
        if (item != null)
        {
            Items.Remove(item);
            _stockService.DeleteItem(item.Id);
            DeletedItems.Add(item);
            OnPropertyChanged(nameof(Items));

            IsReverseEnabled = DeletedItems.Count > 0;
        }
    }

    private void OnReverseClicked(object sender, EventArgs e)
    {
        if (DeletedItems.Count > 0)
        {
            var lastDeletedItem = DeletedItems.Last();
            DeletedItems.Remove(lastDeletedItem);
            Items.Add(lastDeletedItem);//TODO put at the same place before deletion
            _stockService.AddItem(lastDeletedItem);
            OnPropertyChanged(nameof(Items));

            IsReverseEnabled = DeletedItems.Count > 0;
        }
    }
}