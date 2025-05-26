using StockManager.Model;

namespace StockManager.Services
{
    public interface IStockService : IDisposable
    {
        List<Item> GetDefaultItems();
        List<Item> GetItemsFromShopping();
        void UpdateItemToShoppingList(Item item);
        void DeleteItemFromShopping(string errandId);
        void AddItemToShoppingList(Item item);
        string AddNewItemToShoppingList();
        void ClearItemsFromShopping();
        Item? GetItemFromShoppingListById(string Id);
        List<Item> GetItemsFromStock();
        void UpdateItemToStockList(Item item);
        void DeleteItemFromStock(string errandId);
        void AddItemToStockList(Item item);
        string AddNewItemToStockList();
        void ClearItemsFromStock();
        Item? GetItemFromStockListById(string Id);
        void SaveStockListToStockFile();
        List<Item> LoadStockListFromStockFile();
        List<Recipe> GetRecipes();
    }
}
