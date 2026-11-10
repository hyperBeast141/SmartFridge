using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridge
{
    internal class SmartFridge
    {
        public List<Item> Items { get; set; }
        public int MaxCapacity { get; set; }
        public ExpirationManager ExpirationManager { get; set; }
        public ShoppingList ShoppingList { get; set; }
        public UserProfile UserProfile { get; set; }

        public SmartFridge(int maxcapacity)
        {
            MaxCapacity = maxcapacity;
        }

    }
}
