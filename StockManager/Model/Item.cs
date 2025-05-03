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
        public string Location { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool InCart { get; set; }
        public bool InStock { get; set; }
        private bool _isSelected ;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public Item()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Description = string.Empty;
            Quantity = 0;
            Location = string.Empty;
            ExpirationDate = DateTime.Now;
            InCart = false;
            InStock = false;
            IsSelected = false;
        }

        public Item(string id, string name, string description, int quantity, string location, DateTime expirationDate, bool inCart, bool inStock)
        {
            Id = id;
            Name = name;
            Description = description;
            Quantity = quantity;
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
