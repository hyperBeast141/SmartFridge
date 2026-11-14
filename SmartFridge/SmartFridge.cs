using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SmartFridgeProj
{
    internal class SmartFridge
    {
        public List<Item> Items { get; set; }
        public int MaxCapacity { get; set; }
        public ExpirationManager ExpirationManager { get; set; }
        public ShoppingList ShoppingList { get; set; }
        public UserProfile UserProfile { get; set; }
        public Dictionary<string, double> PriceList { get; }

        public int TotalQuantity => Items.Sum(i => i.Quantity);

        public SmartFridge(int maxCapacity, UserProfile userProfile)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException("Capacity must be positive.");

            MaxCapacity = maxCapacity;
            UserProfile = userProfile;
            Items = new List<Item>();
            ShoppingList = new ShoppingList();
            ExpirationManager = new ExpirationManager();
            ExpirationManager.Items = Items;   //  אותה רשימה, לא עותק
            PriceList = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        }
        public void AddItem(Item item)
        {
            if (item == null) throw new ArgumentException("Item cannot be null.");
            if(TotalQuantity + item.Quantity > MaxCapacity)
                throw new InvalidOperationException($"Not enough room. Free space: {MaxCapacity - TotalQuantity}.");

            Item? existing = FindItem(item.Name);

            if(existing != null)
            {
                existing.Quantity += item.Quantity;
                existing.ExpirationDate = item.ExpirationDate;
                existing.Price = item.Price;
            }
            else
            {
                Items.Add(item);
            }
        }
        public bool RemoveItem(string itemName, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            Item? item = FindItem(itemName);

            if (item == null)
                throw new InvalidOperationException($"{itemName} not found in fridge.");

            if (item.Quantity < quantity)
                throw new InvalidOperationException(
                    $"Only {item.Quantity} of {itemName} available.");

            item.Quantity -= quantity;
            return true;
        }

        public Item? FindItem(string itemName)
        {
            return Items.Find(i => string.Equals(i.Name, itemName, StringComparison.OrdinalIgnoreCase));
        }

        public void ReturnItem(string itemName, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            Item? item = FindItem(itemName);

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
            return TotalQuantity >= MaxCapacity;
        }

        public ShoppingList BuildShoppingList(Dictionary<string,int> wanted)
        {
            var list = new ShoppingList();
            foreach (var kv in wanted)
            {
                Item? existing = FindItem(kv.Key);
                int missing = kv.Value - (existing?.Quantity ?? 0);
                if (missing <= 0) continue;

                list.AddToList(kv.Key, missing);

                if (PriceList.TryGetValue(kv.Key, out double price))
                    list.ItemPrices[kv.Key] = price;
                else if (existing != null)
                    list.ItemPrices[kv.Key] = existing.Price;
            }
            list.CalculateTotalCost();
            ShoppingList = list;
            return list;
            
            }
        }

    }

