using SmartFridgeProj;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridgeProj
{
    internal class Program
    {
        static void Main()
        {
            var profile = new UserProfile("User", Diets.None);
            var fridge = new SmartFridge(100,profile);
            /*fridge.AddItem(new FoodItem("Old Cheese", 1, 14, DateTime.Today.AddDays(-1), FoodCategory.Dairy));
            fridge.AddItem(new DrinkItem("Juice", 2, 10, DateTime.Today.AddDays(2), DrinkCategory.Juice));
            profile.AddReminder(new Reminder("Buy milk", "Run out", DateTime.Today, null));*/ //warning alerts test
            new Menu(fridge).Run();

        }
        
    }
}
