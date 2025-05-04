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
        void SaveShoppingListToShoppingFile();
        List<Item> LoadShoppingListFromShoppingFile();
        void ClearItemsFromShopping();
        Item? GetItemFromShoppingListById(string Id);
    }
}
