using StockManager.Model;

namespace StockManager.Services
{
    public interface IStockService : IDisposable
    {
        List<Item> GetDefaultItems();
        List<Item> GetItems();
        void UpdateItem(Item item);
        void DeleteItem(string errandId);
        void AddItem(Item item);
        void AddNewItem();
        void SaveListToFile();
        List<Item> LoadListFromFile();
        void ClearItems();
        Item? GetItemById(string Id);
    }
}
