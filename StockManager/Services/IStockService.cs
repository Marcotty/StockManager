using StockManager.Model;

namespace StockManager.Services
{
    internal interface IStockService
    {
        List<Item> getDefaultItems();
        List<Item> GetItems();
        void UpdateItem(Item item);
        Task DeleteItem(string errandId);
        void AddItem(Item item);
        void AddNewItem();
        void SaveListToFile();
        List<Item> LoadListFromFile();
    }
}
