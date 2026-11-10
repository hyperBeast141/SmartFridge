using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SmartFridge
{
    internal class SmartFridge
    {
        public List<Item> Items { get; set; }
        public int MaxCapacity { get; set; }
        public ExpirationManager ExpirationManager { get; set; }
        public ShoppingList ShoppingList { get; set; }
        public UserProfile UserProfile { get; set; }

        public SmartFridge(int maxCapacity, UserProfile userProfile)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException("Capacity must be positive.");

            MaxCapacity = maxCapacity;
            UserProfile = userProfile;
            Items = new List<Item>();
            ShoppingList = new ShoppingList();
            ExpirationManager = new ExpirationManager();
            ExpirationManager.Items = Items;   //  לא עותק
        }
        public void AddItem(Item item)
        {
            Items.Add(item);
        }
        public bool RemoveItem(string itemName, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            Item? item = Items.Find(i => i.Name == itemName);

            if (item == null)
                throw new InvalidOperationException($"{itemName} not found in fridge.");

            if (item.Quantity < quantity)
                throw new InvalidOperationException(
                    $"Only {item.Quantity} of {itemName} available.");

            item.Quantity -= quantity;
            return true;
        }

        public void ReturnItem(string itemName, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            Item? item = Items.Find(i => i.Name == itemName);

            if (item == null)
                throw new InvalidOperationException($"{itemName} not found in fridge.");

            item.Quantity += quantity;
        }
        public void DisplayInventory()
        {
            foreach (Item item in Items)
            {
                Console.WriteLine($"{item.Name} - {item.Quantity}");
            }
        }
        public bool IsFull()
        {
            return Items.Sum(item => item.Quantity) >= MaxCapacity;
        }
    }
}
