using StockManager.Model;
using StockManager.Services;
using System.Collections.ObjectModel;

namespace StockManager.Pages;

public partial class StoragePage : ContentPage
{
    private readonly IStockService _stockService;
    public ObservableCollection<Item> Stock { get; set; }
    public ObservableCollection<Item> FilteredStock { get; set; }
    public ObservableCollection<Tuple<Item, bool>> SelectedStock { get; set; }
    private List<Item> _allItems;
    public bool IsPanelEnabled { get; set; }
    
    public StoragePage(IStockService stockService)
    {
        InitializeComponent();
        _stockService = stockService;
        _allItems = _stockService.GetDefaultItems();
        Stock = new ObservableCollection<Item>(_allItems);
        FilteredStock = new ObservableCollection<Item>(_allItems);
        SelectedStock = new ObservableCollection<Tuple<Item, bool>>();
        BindingContext = this;
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

    }
}
