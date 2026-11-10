using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridge
{
    internal class ShoppingList
    {
        public Dictionary<string, int> Items { get; set; }

        public Dictionary<string, double> ItemPrices { get; set; }

        public double TotalCost { get; set; }

        public ShoppingList()
        {
            Items = new Dictionary<string, int>();
            ItemPrices = new Dictionary<string, double>();
        }

        public double CalculateTotalCost()
        {
            double sum = 0;
            foreach (var item in Items)
            {
                if (ItemPrices.TryGetValue(item.Key, out double price))
                    sum += item.Value * price;
            }
            TotalCost = sum;
            return sum;
        }
        
        public void AddToList(string itemName, int quantity)
        {
            if (Items.ContainsKey(itemName))
            {
                Items[itemName] += quantity;
            }
            else
            {
                Items.Add(itemName, quantity);
            }
        }
        public void RemoveFromList(string itemName)
        {
            if (Items.ContainsKey(itemName))
            {
                Items.Remove(itemName);
            }
            else
            {
                Console.WriteLine("Item not found in shopping list.");
            }
        }
    }
}
