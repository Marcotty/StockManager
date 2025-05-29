using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Model
{
    public class Item : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public abstract class QuantityUnits
        {
            public const string None = "";
            public const string Kilogram = "kg";
            public const string Gram = "g";
            public const string Liter = "L";
            public const string Milliliter = "ml";

            public static readonly string[] All = { None, Kilogram, Gram, Liter, Milliliter };
        }
        public string QuantityUnit { get; set; }
        public string Location { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool InCart { get; set; }
        public bool InStock { get; set; }
        public bool IsSelected { get; set; }
        private bool _showPanel;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool ShowPanel
        {
            get => _showPanel;
            set
            {
                _showPanel = value;
                OnPropertyChanged(nameof(ShowPanel));
            }
        }

        public Item()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Description = string.Empty;
            Quantity = 0;
            QuantityUnit = QuantityUnits.None;
            Location = string.Empty;
            ExpirationDate = DateTime.Now;
            InCart = false;
            InStock = false;
            IsSelected = false;
        }

        public Item(string id, string name, string description, int quantity, string quantityUnits, string location, DateTime expirationDate, bool inCart, bool inStock)
        {
            Id = id;
            Name = name;
            Description = description;
            Quantity = quantity;
            QuantityUnit = quantityUnits;
            Location = location;
            ExpirationDate = expirationDate;
            InCart = inCart;
            InStock = inStock;
            IsSelected = false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
