﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Model
{
    public class Recipe(string name, string description, List<Item> ingredients, string image, string instructions) : INotifyPropertyChanged
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public string Image { get; set; } = image;
        public string Instructions { get; set; } = instructions;
        public IList<Item> Ingredients { get; set; } = ingredients;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
