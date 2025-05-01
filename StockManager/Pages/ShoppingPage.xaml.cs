using StockManager.Model;
using StockManager.Services;
using System.Collections.ObjectModel;

namespace StockManager.Pages;

public partial class ShoppingPage : ContentPage
{
    private readonly IStockService _stockService;
    public ObservableCollection<Item> Items { get; set; }
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
        //var shoppingPage = _stockService.GetDefaultItems();
        var shoppingPage = _stockService.LoadListFromFile();
        Items = [.. shoppingPage];
        DeletedItems = [];
        IsReverseEnabled = false;
        BindingContext = this;
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is Item item)
        {
            if (_stockService.GetItemById(item.Id) != null)
            {
                item.Name = e.NewTextValue;
                _stockService.UpdateItem(item);
            }
        }
    }

    private void OnLoadShoppingListClicked(object sender, EventArgs e)
    {
        var items = _stockService.LoadListFromFile();
        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(item);
        }
        OnPropertyChanged(nameof(Items));
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

        // Set focus on the last added item
       
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
            DeletedItems.Add(new Tuple<Item, int>(item, Items.IndexOf(item)));
            Items.Remove(item);
            _stockService.DeleteItem(item.Id);
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
            Items.Insert(lastDeletedItem.Item2, lastDeletedItem.Item1);
            _stockService.AddItem(lastDeletedItem.Item1);
            OnPropertyChanged(nameof(Items));

            IsReverseEnabled = DeletedItems.Count > 0;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        try
        {
            _stockService.ClearItems();
            foreach (Item item in Items)
            {
                _stockService.AddItem(item);
            }
            // Save the current state of items to the file
            _stockService.SaveListToFile();

            // Log or perform any cleanup actions
            Console.WriteLine("ShoppingPage is exiting. State saved.");
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the exit process
            Console.WriteLine($"Error during OnDisappearing: {ex.Message}");
        }
    }

    private async void OnClearShoppingListClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm", "Are you sure you want to clear the shopping list?\nThis action is irreversible", "Yes", "No");
        if (confirm)
        {
            Items.Clear();
            _stockService.ClearItems();
        }
    }
}