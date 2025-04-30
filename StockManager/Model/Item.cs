using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Model
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool InStock { get; set; }

        public Item()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Description = string.Empty;
            Quantity = 0;
            Location = string.Empty;
            ExpirationDate = DateTime.Now;
            InStock = false;
        }

        public Item(string id, string name, string description, int quantity, string location, DateTime expirationDate, bool inStock)
        {
            Id = id;
            Name = name;
            Description = description;
            Quantity = quantity;
            Location = location;
            ExpirationDate = expirationDate;
            InStock = inStock;
        }
    }
}
