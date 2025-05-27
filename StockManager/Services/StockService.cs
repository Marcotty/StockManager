using StockManager.Model;
using System.Collections.ObjectModel;
using System.Text;

namespace StockManager.Services
{
    public class StockService : IStockService
    {
        private List<Item> _stockItems;
        private List<Item> _shoppingItems;
        public StockService() 
        {
            _stockItems = LoadStockListFromStockFile();
            var list = new ObservableCollection<Item>();
            foreach (var item in _stockItems)
            {
                if(item.InCart)
                {
                    list.Add(item);
                }
            }
            _shoppingItems = [.. list];
        }

        public List<Recipe> GetRecipes()
        {
            return
            [
                new("Apple Pie", "A delicious apple pie recipe.", new List<Item>
                {
                    new(Guid.NewGuid().ToString(), "Apples", "Fresh apples for pie", 5, "Supermarket A", DateTime.Now.AddDays(7), true, true),
                    new(Guid.NewGuid().ToString(), "Flour", "All-purpose flour", 2, "Supermarket B", DateTime.Now.AddMonths(6), false, true),
                    new(Guid.NewGuid().ToString(), "Sugar", "Granulated sugar", 1, "Supermarket C", DateTime.Now.AddMonths(12), true, false),
                    new(Guid.NewGuid().ToString(), "Butter", "Unsalted butter", 1, "Supermarket D", DateTime.Now.AddDays(15), false, false)
                }, new Image { Source = "dotnet_bot.png"}),
                new("Spaghetti Bolognese", "Classic Italian pasta dish.", new List<Item>
                {
                    new(Guid.NewGuid().ToString(), "Spaghetti", "Dry spaghetti pasta", 1, "Supermarket F", DateTime.Now.AddMonths(12), false, true),
                    new(Guid.NewGuid().ToString(), "Ground Beef", "Fresh ground beef", 1, "Supermarket E", DateTime.Now.AddDays(2), false, false),
                    new(Guid.NewGuid().ToString(), "Tomato Sauce", "Canned tomato sauce", 2, "Supermarket G", DateTime.Now.AddMonths(6), false, true),
                    new(Guid.NewGuid().ToString(), "Onion", "Chopped onion", 1, "Supermarket L", DateTime.Now.AddMonths(2), false, false)
                }, new Image { Source = "dotnet_bot.png"}),
                new("Chicken Curry", "Spicy chicken curry with rice.", new List<Item>
                {
                    new(Guid.NewGuid().ToString(), "Chicken", "Boneless chicken pieces", 1, "Supermarket E", DateTime.Now.AddDays(2), false, false),
                    new(Guid.NewGuid().ToString(), "Curry Powder", "Spicy curry powder", 1, "Supermarket M", DateTime.Now.AddMonths(12), false, true),
                    new(Guid.NewGuid().ToString(), "Coconut Milk", "Canned coconut milk", 1, "Supermarket N", DateTime.Now.AddMonths(6), false, true),
                    new(Guid.NewGuid().ToString(), "Rice", "Basmati rice", 1, "Supermarket D", DateTime.Now.AddMonths(6), false, true)
                }, new Image { Source = "dotnet_bot.png"}),
                new("Vegetable Stir Fry", "Healthy mixed vegetable stir fry.", new List<Item>
                {
                    new(Guid.NewGuid().ToString(), "Broccoli", "Fresh broccoli", 1, "Supermarket H", DateTime.Now.AddDays(5), false, false),
                    new(Guid.NewGuid().ToString(), "Carrots", "Sliced carrots", 2, "Supermarket K", DateTime.Now.AddDays(7), false, false),
                    new(Guid.NewGuid().ToString(), "Bell Pepper", "Red bell pepper", 1, "Supermarket I", DateTime.Now.AddDays(6), false, false),
                    new(Guid.NewGuid().ToString(), "Soy Sauce", "Bottled soy sauce", 1, "Supermarket O", DateTime.Now.AddMonths(12), false, true)
                }, new Image { Source = "dotnet_bot.png"}),
                new("Omelette", "Simple cheese and ham omelette.", new List<Item>
                {
                    new(Guid.NewGuid().ToString(), "Eggs", "Fresh eggs", 3, "Supermarket B", DateTime.Now.AddDays(10), false, false),
                    new(Guid.NewGuid().ToString(), "Cheese", "Grated cheddar cheese", 1, "Supermarket B", DateTime.Now.AddDays(20), false, false),
                    new(Guid.NewGuid().ToString(), "Ham", "Sliced ham", 1, "Supermarket P", DateTime.Now.AddDays(7), false, false),
                    new(Guid.NewGuid().ToString(), "Milk", "Whole milk", 1, "Supermarket B", DateTime.Now.AddDays(5), false, false)
                }, new Image { Source = "dotnet_bot.png"}),
                new("Fruit Salad", "Refreshing mixed fruit salad.", new List<Item>
                {
                    new(Guid.NewGuid().ToString(), "Bananas", "Fresh bananas", 2, "Supermarket H", DateTime.Now.AddDays(3), false, false),
                    new(Guid.NewGuid().ToString(), "Oranges", "Juicy oranges", 2, "Supermarket I", DateTime.Now.AddDays(6), false, false),
                    new(Guid.NewGuid().ToString(), "Apples", "Crisp apples", 2, "Supermarket A", DateTime.Now.AddDays(7), false, false),
                    new(Guid.NewGuid().ToString(), "Yogurt", "Plain yogurt", 1, "Supermarket N", DateTime.Now.AddDays(10), false, false)
                }, new Image { Source = "dotnet_bot.png"})
            ];
        }

        public void AddItemToStockList(Item item)
        {
            _stockItems.Add(item);
        }

        public void AddItemToShoppingList(Item item)
        {
            if(!_shoppingItems.Contains(item))
                _shoppingItems.Add(item);
        }

        public string AddNewItemToStockList()
        {
            string id = Guid.NewGuid().ToString();
            AddItemToStockList(new Item
            {
                Id = id,
                Name = string.Empty,
                Description = string.Empty,
                Quantity = 0,
                Location = string.Empty,
                ExpirationDate = DateTime.Now,
                InCart = false,
                InStock = false
            });
            return id;
        }

        public string AddNewItemToShoppingList()
        {
            string id = Guid.NewGuid().ToString();
            AddItemToShoppingList(new Item
            {
                Id = id,
                Name = string.Empty,
                Description = string.Empty,
                Quantity = 0,
                Location = string.Empty,
                ExpirationDate = DateTime.Now,
                InCart = false,
                InStock = false
            });
            return id;
        }

        public Item? GetItemFromShoppingListById(string Id)
        {
            return _shoppingItems.FirstOrDefault(i => i.Id == Id);
        }

        public Item? GetItemFromStockListById(string Id)
        {
            return _stockItems.FirstOrDefault(i => i.Id == Id);
        }

        public void ClearItemsFromStock()
        {
            _stockItems.Clear();
        }

        public void ClearItemsFromShopping()
        {
            _shoppingItems.Clear();
        }

        public void DeleteItemFromStock(string ItemId)
        {
            var item = _stockItems.FirstOrDefault(i => i.Id == ItemId);
            if (item != null)
            {
                _stockItems.Remove(item);
            }
        }

        public void DeleteItemFromShopping(string ItemId)
        {
            var item = _shoppingItems.FirstOrDefault(i => i.Id == ItemId);
            if (item != null)
            {
                _shoppingItems.Remove(item);
            }
        }

        public List<Item> GetItemsFromStock()
        {
            return _stockItems;
        }

        public List<Item> GetItemsFromShopping()
        {
            return _shoppingItems;
        }

        public void UpdateItemToStockList(Item newItem)
        {
            Item? item = _stockItems.FirstOrDefault(i => i.Id == newItem.Id);
            if (item == null)
            {
                newItem.Id = Guid.NewGuid().ToString();
                _stockItems.Add(newItem);
            }
            else
            {
                item.Name = newItem.Name;
                item.Description = newItem.Description;
                item.Quantity = newItem.Quantity;
                item.Location = newItem.Location;
                item.ExpirationDate = newItem.ExpirationDate;
                item.InCart = newItem.InCart;
                item.InStock = newItem.InStock;
            }
        }

        public void UpdateItemToShoppingList(Item newItem)
        {
            Item? item = _shoppingItems.FirstOrDefault(i => i.Id == newItem.Id);
            if (item == null)
            {
                newItem.Id = Guid.NewGuid().ToString();
                _shoppingItems.Add(newItem);
            }
            else
            {
                item.Name = newItem.Name;
                item.Description = newItem.Description;
                item.Quantity = newItem.Quantity;
                item.Location = newItem.Location;
                item.ExpirationDate = newItem.ExpirationDate;
                item.InCart = newItem.InCart;
                item.InStock = newItem.InStock;
            }
        }

        public List<Item> LoadStockListFromStockFile()
        {
            if (_stockItems != null)
                return _stockItems;
            try
            {
                // Define the file path to load the list
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "stock_data.txt");

                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Close();
                        _stockItems ??= [];
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
                        var inCart = bool.Parse(lines[6].Replace("In Cart: ", "").Trim());
                        var inStock = bool.Parse(lines[7].Replace("In Stock: ", "").Trim());

                        var item = new Item
                        {
                            Id = id,
                            Name = name,
                            Description = description,
                            Quantity = quantity,
                            Location = location,
                            ExpirationDate = expirationDate,
                            InCart = inCart,
                            InStock = inStock
                        };
                        _stockItems ??= [];
                        _stockItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while loading the stock: {ex.Message}");
            }
            return _stockItems ?? [];
        }

        public void SaveStockListToStockFile()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "stock_data.txt");

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }
                var stringBuilder = new StringBuilder();
                foreach (var item in _stockItems)
                {
                    if (item.Name != string.Empty)
                    {
                        stringBuilder.AppendLine($"Id: {item.Id}");
                        stringBuilder.AppendLine($"Name: {item.Name}");
                        stringBuilder.AppendLine($"Description: {item.Description}");
                        stringBuilder.AppendLine($"Quantity: {item.Quantity}");
                        stringBuilder.AppendLine($"Location: {item.Location}");
                        stringBuilder.AppendLine($"Expiration Date: {item.ExpirationDate.ToShortDateString()}");
                        stringBuilder.AppendLine($"In Cart: {item.InCart}");
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

        public List<Item> GetDefaultItems()
        {
            return new List<Item>
           {
               new Item(Guid.NewGuid().ToString(), "Apples", "Red apples, 1 kg", 10, "Supermarket A", DateTime.Now.AddDays(7), true, false),
               new Item(Guid.NewGuid().ToString(), "Milk", "2 liters of whole milk", 5, "Supermarket B", DateTime.Now.AddDays(5), false, false),
               new Item(Guid.NewGuid().ToString(), "Bread", "Whole grain bread", 3, "Supermarket C", DateTime.Now.AddDays(3), true, false),
               new Item(Guid.NewGuid().ToString(), "Eggs", "1 dozen eggs", 12, "Supermarket B", DateTime.Now.AddDays(10), false, false),
               new Item(Guid.NewGuid().ToString(), "Rice", "Basmati rice, 5 kg", 2, "Supermarket D", DateTime.Now.AddMonths(6), false, false),
               new Item(Guid.NewGuid().ToString(), "Chicken", "Fresh chicken, 2 kg", 1, "Supermarket E", DateTime.Now.AddDays(2), true, false),
               new Item(Guid.NewGuid().ToString(), "Butter", "Salted butter, 500 g", 4, "Supermarket B", DateTime.Now.AddDays(15), false, false),
               new Item(Guid.NewGuid().ToString(), "Pasta", "Spaghetti, 1 kg", 6, "Supermarket F", DateTime.Now.AddMonths(12), true, false),
               new Item(Guid.NewGuid().ToString(), "Tomatoes", "Fresh tomatoes, 1 kg", 8, "Supermarket G", DateTime.Now.AddDays(4), false, false),
               new Item(Guid.NewGuid().ToString(), "Cheese", "Cheddar cheese, 500 g", 2, "Supermarket B", DateTime.Now.AddDays(20), false, false),
               new Item(Guid.NewGuid().ToString(), "Bananas", "1 kg", 7, "Supermarket H", DateTime.Now.AddDays(3), true, false),
               new Item(Guid.NewGuid().ToString(), "Oranges", "1 kg", 5, "Supermarket I", DateTime.Now.AddDays(6), false, false),
               new Item(Guid.NewGuid().ToString(), "Potatoes", "White potatoes, 2 kg", 10, "Supermarket J", DateTime.Now.AddMonths(1), true, false),
               new Item(Guid.NewGuid().ToString(), "Carrots", "Fresh carrots, 1 kg", 6, "Supermarket K", DateTime.Now.AddDays(7), false, false),
               new Item(Guid.NewGuid().ToString(), "Onions", "Red onions, 1 kg", 8, "Supermarket L", DateTime.Now.AddMonths(2), true, false),
               new Item(Guid.NewGuid().ToString(), "Garlic", "Fresh garlic, 500 g", 3, "Supermarket M", DateTime.Now.AddMonths(3), false, false),
               new Item(Guid.NewGuid().ToString(), "Yogurt", "Plain yogurt, 1 kg", 4, "Supermarket N", DateTime.Now.AddDays(10), true, false),
               new Item(Guid.NewGuid().ToString(), "Cereal", "Breakfast cereal, 500 g", 5, "Supermarket O", DateTime.Now.AddMonths(6), false, false),
               new Item(Guid.NewGuid().ToString(), "Juice", "Orange juice, 1 liter", 6, "Supermarket P", DateTime.Now.AddDays(7), true, false),
               new Item(Guid.NewGuid().ToString(), "Coffee", "Ground coffee, 250 g", 2, "Supermarket Q", DateTime.Now.AddMonths(12), false, false)
           };
        }

        public void Dispose()
        {
            SaveStockListToStockFile();
        }
    }
}
