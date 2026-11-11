using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridgeProj
{
    internal class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Item(string name, int quantity, double price, DateTime expirationDate)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            ExpirationDate = expirationDate;
        }

    }
}

