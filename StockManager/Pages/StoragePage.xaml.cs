using StockManager.Model;
using StockManager.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockManager.Pages;

public partial class StoragePage : ContentPage
{
    private readonly IStockService _stockService;
    public ObservableCollection<Item> Stock { get; set; }
    public ObservableCollection<Item> FilteredStock { get; set; }
    public ObservableCollection<Tuple<Item, bool>> SelectedStock { get; set; }
    public ICommand ItemTappedCommand => new Command<Item>(OnItemTapped);
    private List<Item> _allItems;
    public bool IsPanelEnabled { get; set; }
    private bool _isFrameVisible;
    public bool IsFrameVisible
    {
        get => _isFrameVisible;
        set
        {
            _isFrameVisible = value;
            OnPropertyChanged();
        }
    }

    private Item? _selectedItem;
    public Item? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    public StoragePage(IStockService stockService)
    {
        InitializeComponent();
        _stockService = stockService;
        _allItems = _stockService.GetDefaultItems();
        Stock = new ObservableCollection<Item>(_allItems);
        FilteredStock = new ObservableCollection<Item>(_allItems);
        SelectedStock = new ObservableCollection<Tuple<Item, bool>>();
        IsFrameVisible = false;
        IsPanelEnabled = false;
        BindingContext = this;
    }

    private async void OnAddItem(object sender, EventArgs e)
    {
        string id = _stockService.AddNewItemToStockList();
        Item? newItem = _stockService.GetItemFromStockListById(id);
        if (newItem != null)
        {
            await Navigation.PushAsync(new EditItemInStock(_stockService, newItem));
            FilteredStock.Clear();
            FilteredStock = new ObservableCollection<Item>(_stockService.GetItemsFromShopping());
            FilteredStock.Remove(newItem);
            FilteredStock.Insert(0, newItem);
            OnPropertyChanged(nameof(FilteredStock));
        }
    }

    private void OnDeleteItemClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Item item)
        {
            // Remove the item from the stock
            _stockService.DeleteItemFromStock(item.Id);
            FilteredStock.Remove(item);
            Stock.Remove(item);
            _allItems.Remove(item);
            _selectedItem = null;
            OnPropertyChanged(nameof(FilteredStock));
        }
    }

    private async void OnAddToCartItems(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm", "Add selected items to shopping list ?", "Yes", "No");
        if (confirm)
        {
            foreach (var item in SelectedStock)
            {
                if (item.Item2)
                {
                    item.Item1.IsSelected = false;
                    item.Item1.InCart = true;
                    _stockService.AddItemToShoppingList(item.Item1);
                }
            }

            confirm = await DisplayAlert("Confirm", "Items added to shopping list. \n\n Go to shopping list ?", "Yes", "No");
            if (confirm)
            {
               await Navigation.PushAsync(new ShoppingPage(_stockService));
            }
        }
    }
    private void OnDeleteItems(object sender, EventArgs e)
    {
        // Handle delete items logic
        foreach (var item in SelectedStock)
        {
            if (item.Item2)
            {
                _stockService.DeleteItemFromStock(item.Item1.Id);
                FilteredStock.Remove(item.Item1);
                Stock.Remove(item.Item1);
                _allItems.Remove(item.Item1);
            }
        }
        SelectedStock.Clear();
        OnPropertyChanged(nameof(FilteredStock));
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        // Pass the search query to the filtering method
        OnSearchQueryChanged(e.NewTextValue);
    }

    public void OnSearchQueryChanged(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            // Reset the filtered list to show all items
            FilteredStock.Clear();
            foreach (var item in _allItems)
            {
                FilteredStock.Add(item);
            }
        }
        else
        {
            // Filter items based on the search query
            var filteredItems = _allItems
                .Where(item => item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                               item.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredStock.Clear();
            foreach (var item in filteredItems)
            {
                FilteredStock.Add(item);
            }
        }
        OnPropertyChanged(nameof(FilteredStock));
    }

    private void OnSortCriteriaChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
        {
            string? selectedCriteria = picker.SelectedItem?.ToString();
            foreach (var item in FilteredStock)
            {
                Console.WriteLine($"Before Item: {item.Name}, {item.Description}");
            }

            SortFilteredStock(selectedCriteria);
            foreach (var item in FilteredStock)
            {
                Console.WriteLine($"After Item: {item.Name}, {item.Description}");
            }
        }
    }

    private async void OnItemTapped(Item selectedItem)
    {
        selectedItem.ShowPanel = !selectedItem.ShowPanel;
        // Animate the panel visibility
        var frame = FindFrameForItem(selectedItem); // Helper method to locate the Frame
        if (frame != null)
        {
            if (selectedItem.ShowPanel)
            {
                // Animate showing the panel
                frame.Opacity = 0;
                frame.IsVisible = true;
                await frame.FadeTo(1, 250); // Fade in over 250ms
                await frame.TranslateTo(0, 0, 250, Easing.SinOut); // Slide in
            }
            else
            {
                // Animate hiding the panel
                await frame.FadeTo(0, 200); // Fade out over 200ms
                await frame.TranslateTo(0, 20, 250, Easing.SinOut); // Slide out
                frame.IsVisible = false;
            }
        }
        /* // Hide other panels
        foreach (var item in FilteredStock)
        {
            if (item != selectedItem)
            {
                item.ShowPanel = false;
                frame = FindFrameForItem(selectedItem);
                await frame.FadeTo(0, 200); // Fade out over 200ms
                await frame.TranslateTo(0, 20, 250, Easing.SinOut); // Slide out
                frame.IsVisible = false;
            }
        }*/
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        // Handle edit button click
        if (sender is Button button && button.BindingContext is Item item)
        {
            // Navigate to the edit page or perform edit logic
            await Navigation.PushAsync(new EditItemInStock(_stockService, item));
            FilteredStock.Clear();
            FilteredStock = new ObservableCollection<Item>(_stockService.GetItemsFromStock());
            OnPropertyChanged(nameof(FilteredStock));
        }
    }

    private Frame? FindFrameForItem(Item item)
    {
        // Assuming the CollectionView is named "collectionView"
        var collectionView = this.FindByName<CollectionView>("collectionView");
        if (collectionView == null) return null;

        // Workaround for the missing 'ContainerFromItem' method
        foreach (var visualElement in collectionView.LogicalChildren.OfType<VisualElement>())
        {
            if (visualElement.BindingContext == item)
            {
                return visualElement.FindByName<Frame>("DetailsFrame"); // Replace "DetailsFrame" with the x:Name of your Frame
            }
        }

        return null;
    }

    private void SortFilteredStock(string? criteria)
    {
        if (string.IsNullOrEmpty(criteria)) return;

        List<Item> sortedItems = criteria switch
        {
            "Name" => FilteredStock.OrderBy(item => item.Name).ToList(),
            "Quantity" => FilteredStock.OrderBy(item => item.Quantity).ToList(),
            "Expiration Date" => FilteredStock.OrderBy(item => item.ExpirationDate).ToList(),
            _ => FilteredStock.ToList()
        };

        // Update the FilteredStock collection
        FilteredStock.Clear();
        foreach (var item in sortedItems)
        {
            FilteredStock.Add(item);
        }

        OnPropertyChanged(nameof(FilteredStock));
    }

    private void SelectCheckBoxChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is Item item)
        {
            // Add or remove the item from the SelectedStock collection
            if (item.IsSelected)
            {
                SelectedStock.Add(new Tuple<Item, bool>(item, true));
            }
            else
            {
                var selectedItem = SelectedStock.FirstOrDefault(i => i.Item1.Id == item.Id);
                if (selectedItem != null)
                {
                    SelectedStock.Remove(selectedItem);
                }
            }
        }
    }
}
