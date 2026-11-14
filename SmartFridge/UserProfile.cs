using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridgeProj
{
    public enum Diets
    {
        None,
        Vegetarian,
        Vegan,
        GlutenFree,
        DairyFree,
        Kosher
    }
    internal class UserProfile
    {
        public string Username { get; set; }
        public Diets Diet { get; set; }
        public List<Item> FavoriteItems { get; set; }
        public List<Reminder> Reminders { get; set; }
        public Dictionary<string, string> Notes { get; set; }//<title ,description>

        public UserProfile(string username, Diets diet)
        {
            Username = username;
            Diet = diet;
            FavoriteItems = new List<Item>();
            Reminders = new List<Reminder>();
            Notes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        public void AddFavorite(Item item)
        {
            FavoriteItems.Add(item);
        }
        public void RemoveFavorite(Item item)
        {
            FavoriteItems.Remove(item);
        }
        public void DisplayFavorites()
        {
            foreach(var item in FavoriteItems)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine($"Quantity: {item.Quantity}");

            }
        }
        public void SetDiet(Diets diet)
        {
            Diet = diet;
        }
        public void AddReminder(Reminder reminder)
        {
            Reminders.Add(reminder);
        }
        public void RemoveReminder(Reminder reminder)
        {
            Reminders.Remove(reminder);

        }
        public List<Reminder> GetDueReminders()
        {
            return Reminders.FindAll(r => r.ReminderDate.Date <= DateTime.Today);
        }
        public void DisplayReminder()
        {
            foreach (var reminder in Reminders)
            {
                Console.WriteLine($"Description: {reminder.Description}");
                Console.WriteLine($"Date: {reminder.ReminderDate:dd/MM/yyyy}");
                if (reminder.RelatedItem != null)
                    Console.WriteLine($"Related item: {reminder.RelatedItem.Name}");
            }
        }
        public void AddNote(string itemName, string note)
        {
            Notes[itemName] = note;
        }
        public string GetNote(string itemName)
        {
            if (Notes.ContainsKey(itemName))
            {
                return Notes[itemName];
            }

            return "No note found for this item.";
        }

        public string? GetDietWarning(Item item)
        {
            bool isMeat = item is FoodItem food && food.Category == FoodCategory.Meat;
            bool isDairy = (item is FoodItem f2 && f2.Category == FoodCategory.Dairy)
                        || (item is DrinkItem drink && drink.Category == DrinkCategory.Dairy);

            if (Diet == Diets.Vegetarian && isMeat)
                return $"{item.Name} contains meat, but your diet is Vegetarian.";
            if (Diet == Diets.Vegan && (isMeat || isDairy))
                return $"{item.Name} is not vegan.";
            if (Diet == Diets.DairyFree && isDairy)
                return $"{item.Name} contains dairy, but your diet is Dairy-free.";
            return null;
        }
    }
}
