using System;
using System.Collections.Generic;
using System.Text;
public enum DrinkCategory
{
    Dairy,
    Soda,//שתייה קלה
    Juice,//מיצים
    Other
}
namespace SmartFridgeProj
{
    internal class DrinkItem : Item
    {
        public DrinkCategory Category { get; set; }
        public DrinkItem(string name, int quantity, double price,
            DateTime expirationDate, DrinkCategory category)
           : base(name, quantity, price, expirationDate)
        {
            Category = category;
        }

    }
}
