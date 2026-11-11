using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
public enum FoodCategory
{
    Dairy,
    Meat,
    Vegetables,
    Fruits,
    Bakery,
    Frozen,
    Other
}

namespace SmartFridgeProj
{
    internal class FoodItem : Item
    {
        public FoodCategory Category { get; set; }

        public FoodItem(string name, int quantity, double price, DateTime expirationDate,FoodCategory category)
            : base(name, quantity, price, expirationDate)
        {
            Category = category;
        }
    }
}
