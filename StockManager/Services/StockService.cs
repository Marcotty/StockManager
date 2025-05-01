using StockManager.Model;
using System.Text;

namespace StockManager.Services
{
    public class StockService : IStockService
    {
        private List<Item> _items;
        public StockService() 
        {
            _items = LoadListFromFile();
        }
        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public void AddNewItem()
        {
            AddItem(new Item
            {
                Id = Guid.NewGuid().ToString(),
                Name = string.Empty,
                Description = string.Empty,
                Quantity = 0,
                Location = string.Empty,
                ExpirationDate = DateTime.Now,
                InStock = false
            });
        }

        public Item? GetItemById(string Id)
        {
            return _items.FirstOrDefault(i => i.Id == Id);
        }

        public void ClearItems()
        {
            _items.Clear();
        }

        public void DeleteItem(string ItemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == ItemId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public List<Item> GetItems()
        {
            return _items;
        }

        public List<Item> LoadListFromFile()
        {
            if (_items != null)
                return _items;
            try
            {
                // Define the file path to load the list
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "Stock.txt");

                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Close();
                        _items ??= [];
                    }
                }
                else
                {
                    // Read the file content
                    var fileContent = File.ReadAllText(filePath);

                    // Parse the file content and populate the errands list
                    var itemsData = fileContent.Split(new[] { new string('-', 20) }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var errandData in itemsData)
                    {
                        var lines = errandData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        if (lines.Length < 4) continue;

                        var id = lines[0].Replace("Id: ", "").Trim();
                        var name = lines[1].Replace("Name: ", "").Trim();
                        var description = lines[2].Replace("Description: ", "").Trim();
                        var quantity = int.Parse(lines[3].Replace("Quantity: ", "").Trim());
                        var location = lines[4].Replace("Location: ", "").Trim();
                        var expirationDate = DateTime.Parse(lines[5].Replace("Expiration Date: ", "").Trim());
                        var inStock = bool.Parse(lines[6].Replace("In Stock: ", "").Trim());

                        var item = new Item
                        {
                            Name = name,
                        };
                        _items ??= [];
                        _items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while loading the stock: {ex.Message}");
            }
            return _items ?? [];
        }

        public void SaveListToFile()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "Stock.txt");

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }
                var stringBuilder = new StringBuilder();
                foreach (var item in _items)
                {
                    if (item.Name != string.Empty)
                    {
                        stringBuilder.AppendLine($"Id: {item.Id}");
                        stringBuilder.AppendLine($"Name: {item.Name}");
                        stringBuilder.AppendLine($"Description: {item.Description}");
                        stringBuilder.AppendLine($"Quantity: {item.Quantity}");
                        stringBuilder.AppendLine($"Location: {item.Location}");
                        stringBuilder.AppendLine($"Expiration Date: {item.ExpirationDate.ToShortDateString()}");
                        stringBuilder.AppendLine($"In Stock: {item.InStock}");
                        stringBuilder.AppendLine(new string('-', 20));
                    }
                }

                File.WriteAllText(filePath, stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving the stock: {ex.Message}");
            }
        }

        public void UpdateItem(Item newItem)
        {
            Item? item = _items.FirstOrDefault(i => i.Id == newItem.Id);
            if (item == null)
            {
                newItem.Id = Guid.NewGuid().ToString();
                _items.Add(newItem);
            }
            else
            {
                item.Name = newItem.Name;
                item.Description = newItem.Description;
                item.Quantity = newItem.Quantity;
                item.Location = newItem.Location;
                item.ExpirationDate = newItem.ExpirationDate;
                item.InStock = newItem.InStock;
            }
        }

        public List<Item> GetDefaultItems()
        {
            _items =
           [
               new Item(Guid.NewGuid().ToString(), "Apples", "Red apples, 1 kg", 10, "Supermarket A", DateTime.Now.AddDays(7), true),
               new Item(Guid.NewGuid().ToString(), "Milk", "2 liters of whole milk", 5, "Supermarket B", DateTime.Now.AddDays(5), false),
               new Item(Guid.NewGuid().ToString(), "Bread", "Whole grain bread", 3, "Supermarket C", DateTime.Now.AddDays(3), true),
               new Item(Guid.NewGuid().ToString(), "Eggs", "1 dozen eggs", 12, "Supermarket B", DateTime.Now.AddDays(10), false),
               new Item(Guid.NewGuid().ToString(), "Rice", "Basmati rice, 5 kg", 2, "Supermarket D", DateTime.Now.AddMonths(6), false),
               new Item(Guid.NewGuid().ToString(), "Chicken", "Fresh chicken, 2 kg", 1, "Supermarket E", DateTime.Now.AddDays(2), true),
               new Item(Guid.NewGuid().ToString(), "Butter", "Salted butter, 500 g", 4, "Supermarket B", DateTime.Now.AddDays(15), false),
               new Item(Guid.NewGuid().ToString(), "Pasta", "Spaghetti, 1 kg", 6, "Supermarket F", DateTime.Now.AddMonths(12), true),
               new Item(Guid.NewGuid().ToString(), "Tomatoes", "Fresh tomatoes, 1 kg", 8, "Supermarket G", DateTime.Now.AddDays(4), false),
               new Item(Guid.NewGuid().ToString(), "Cheese", "Cheddar cheese, 500 g", 2, "Supermarket B", DateTime.Now.AddDays(20), false),
               new Item(Guid.NewGuid().ToString(), "Bananas", "Yellow bananas, 1 kg", 7, "Supermarket H", DateTime.Now.AddDays(3), true),
               new Item(Guid.NewGuid().ToString(), "Oranges", "Fresh oranges, 1 kg", 5, "Supermarket I", DateTime.Now.AddDays(6), false),
               new Item(Guid.NewGuid().ToString(), "Potatoes", "White potatoes, 2 kg", 10, "Supermarket J", DateTime.Now.AddMonths(1), true),
               new Item(Guid.NewGuid().ToString(), "Carrots", "Fresh carrots, 1 kg", 6, "Supermarket K", DateTime.Now.AddDays(7), false),
               new Item(Guid.NewGuid().ToString(), "Onions", "Red onions, 1 kg", 8, "Supermarket L", DateTime.Now.AddMonths(2), true),
               new Item(Guid.NewGuid().ToString(), "Garlic", "Fresh garlic, 500 g", 3, "Supermarket M", DateTime.Now.AddMonths(3), false),
               new Item(Guid.NewGuid().ToString(), "Yogurt", "Plain yogurt, 1 kg", 4, "Supermarket N", DateTime.Now.AddDays(10), true),
               new Item(Guid.NewGuid().ToString(), "Cereal", "Breakfast cereal, 500 g", 5, "Supermarket O", DateTime.Now.AddMonths(6), false),
               new Item(Guid.NewGuid().ToString(), "Juice", "Orange juice, 1 liter", 6, "Supermarket P", DateTime.Now.AddDays(7), true),
               new Item(Guid.NewGuid().ToString(), "Coffee", "Ground coffee, 250 g", 2, "Supermarket Q", DateTime.Now.AddMonths(12), false)
           ];
            return _items;
        }

        public void Dispose()
        {
            SaveListToFile();
        }
    }
}
