using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridgeProj
{
    internal class Item
    {
        public string Name { get; set; }
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be negative.");
                _quantity = value;
            }
        }//Quantity > 0
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

