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
            Notes = new Dictionary<string, string>();
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
        public void DisplayReminder()
        {
            foreach (var reminder in Reminders)
            {
                Console.WriteLine(reminder.Title);
                Console.WriteLine($"Description:{reminder.Description}");
                Console.WriteLine($"Date: {reminder.ReminderDate}");
                if(reminder.RelatedItem != null)
                    Console.WriteLine($"Related Item{reminder.RelatedItem}");
            }
        }
        public void AddNote(string itemName, string note)
        {
            Notes.Add(itemName, note);
        }
        public string GetNote(string itemName)
        {
            if (Notes.ContainsKey(itemName))
            {
                return Notes[itemName];
            }

            return "No note found for this item.";
        }

    }
}
