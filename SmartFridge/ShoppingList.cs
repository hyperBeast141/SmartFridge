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
                TotalCost += item.Value * ItemPrices[item.Key];
            }


            return sum;
        }
    }
}
