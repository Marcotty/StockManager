using StockManager.Model;
using StockManager.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StockManager.Pages;

public partial class ShoppingPage : ContentPage
{
    private readonly IStockService _stockService;
    public ObservableCollection<Item> Items { get; set; }
    public ObservableCollection<Item> FilteredItems { get; set; }
    public ObservableCollection<Tuple<Item, int>> DeletedItems { get; set; }
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
        var shoppingData = _stockService.GetItemsFromShopping();
        Items = [.. shoppingData];
        FilteredItems = [.. Items];
        DeletedItems = [];
        IsReverseEnabled = false;
        BindingContext = this;
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is Item item)
        {
            if (_stockService.GetItemFromShoppingListById(item.Id) != null)
            {
                item.Name = e.NewTextValue;
                _stockService.UpdateItemToShoppingList(item);
            }
        }
    }

    private void OnLoadShoppingListClicked(object sender, EventArgs e)
    {
        var items = _stockService.LoadStockListFromStockFile();
        Items.Clear();
        foreach (var item in items)
        {
            if (item.InCart)
                Items.Add(item);
        }
        FilteredItems.Clear();
        foreach (var item in Items)
        {
            FilteredItems.Add(item);
        }
        OnPropertyChanged(nameof(FilteredItems));
    }

    private void OnAddItem(object sender, EventArgs e)
    {
        var newItem = new Item
        {
            Id = string.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Quantity = 0,
            QuantityUnit = Item.QuantityUnits.None,
            Location = string.Empty,
            ExpirationDate = DateTime.Now,
            InCart = true,
            InStock = false,
        };
        Items.Add(newItem);
        FilteredItems.Add(newItem);
        _stockService.AddNewItemToShoppingList();
        OnPropertyChanged(nameof(FilteredItems));       
    }

    private void OnValidateClicked(object sender, EventArgs e)
    {
        var itemsToRemove = Items.Where(item => item.IsSelected).ToList();
        foreach (var item in itemsToRemove)
        {
            Items.Remove(item);
            FilteredItems.Remove(item);
            item.InCart = false;
            item.InStock = true;
            item.IsSelected = false;
            _stockService.UpdateItemToStockList(item);
        }
        OnPropertyChanged(nameof(FilteredItems));
    }

    private void OnCheckBoxAllSelectedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is not CheckBox checkBox)
            return;
        foreach (var item in FilteredItems)
        {
            item.IsSelected = checkBox.IsChecked;
            _stockService.UpdateItemToShoppingList(item);
        }
        OnPropertyChanged(nameof(FilteredItems));
    }

    private void OnSortCriteriaChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
        {
            string? selectedCriteria = picker.SelectedItem?.ToString();
            foreach (var item in FilteredItems)
            {
                Console.WriteLine($"Before Item: {item.Name}, {item.Description}");
            }

            SortFilteredStock(selectedCriteria);
            foreach (var item in FilteredItems)
            {
                Console.WriteLine($"After Item: {item.Name}, {item.Description}");
            }
        }
    }

    private void SortFilteredStock(string? criteria)
    {
        if (string.IsNullOrEmpty(criteria)) return;

        List<Item> sortedItems = criteria switch
        {
            "Name" => FilteredItems.OrderBy(item => item.Name).ToList(),
            "Quantity" => FilteredItems.OrderBy(item => item.Quantity).ToList(),
            _ => FilteredItems.ToList()
        };

        FilteredItems.Clear();
        foreach (var item in sortedItems)
        {
            FilteredItems.Add(item);
        }

        OnPropertyChanged(nameof(FilteredItems));
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
            item.IsSelected = checkBox.IsChecked;
            _stockService.UpdateItemToShoppingList(item);
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
            DeletedItems.Add(new Tuple<Item, int>(item, FilteredItems.IndexOf(item)));
            FilteredItems.Remove(item);
            Items.Remove(item);
            _stockService.DeleteItemFromShopping(item.Id);
            OnPropertyChanged(nameof(FilteredItems));

            IsReverseEnabled = DeletedItems.Count > 0;
        }
    }

    private void OnReverseClicked(object sender, EventArgs e)
    {
        if (DeletedItems.Count > 0)
        {
            var lastDeletedItem = DeletedItems.Last();
            DeletedItems.Remove(lastDeletedItem);
            Items.Insert(lastDeletedItem.Item2, lastDeletedItem.Item1);
            FilteredItems.Insert(lastDeletedItem.Item2, lastDeletedItem.Item1);
            _stockService.AddItemToShoppingList(lastDeletedItem.Item1);
            OnPropertyChanged(nameof(FilteredItems));

            IsReverseEnabled = DeletedItems.Count > 0;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Items.Clear();
        FilteredItems.Clear();
        Items = new ObservableCollection<Item>(_stockService.GetItemsFromShopping());
        foreach (var item in Items)
        {
            FilteredItems.Add(item);
        }
        OnPropertyChanged(nameof(FilteredItems));
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        try
        {
            _stockService.ClearItemsFromShopping();
            foreach (Item item in Items)
            {
                _stockService.AddItemToShoppingList(item);
                _stockService.UpdateItemToStockList(item);
            }
            Console.WriteLine("ShoppingPage is exiting. State saved.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during OnDisappearing: {ex.Message}");
        }
    }

    private async void OnClearShoppingListClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm", "Are you sure you want to clear the shopping list?\nThis action is irreversible", "Yes", "No");
        if (confirm)
        {
            Items.Clear();
            _stockService.ClearItemsFromShopping();
            FilteredItems.Clear();
        }
        OnPropertyChanged(nameof(FilteredItems));
    }
}