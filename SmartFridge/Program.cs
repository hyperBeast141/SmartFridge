using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SmartFridgeProj
{
    internal class Program
    {
        private readonly SmartFridge _fridge;

        public Menu(SmartFridge fridge)
        {
            _fridge = fridge;
        }
        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("     Smart Fridge      ");
            Console.WriteLine("1. View inventory");
            Console.WriteLine("2. Add item");
            Console.WriteLine("3. Take item out");
            Console.WriteLine("4. Return item");
            Console.WriteLine("5. Expiring / expired items");
            Console.WriteLine("6. Build shopping list");
            Console.WriteLine("7. Reminders");
            Console.WriteLine("8. Notes");
            Console.WriteLine("9. Favorites");
            Console.WriteLine("0. Exit");
        }
        public void run()
        {
            Console.WriteLine($"{GetGreeting()}, {_fridge.UserProfile.Username}!");
            ShowStartupAlerts();
            bool running = true;

            while (running)
            {
                ShowMenu();
                int choice = InputHelper.ReadInt("Choose an option: ");

                try
                {
                    switch (choice)
                    {
                        case 1: ShowInventory(); break;
                        case 2: AddItem(); break;
                        case 3: TakeItemOut(); break;
                        case 4: ReturnItem(); break;
                        case 5: ShowExpiringItems(); break;
                        case 6: BuildShoppingList(); break;
                        case 7: RemindersMenu(); break;
                        case 8: NotesMenu(); break;
                        case 9: FavoritesMenu(); break;
                        case 0: running = false; break;
                        default: Console.WriteLine("Invalid Option,try again."); break;
                    }
                }
                catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                if (running) Pause();
            }

            Console.WriteLine("Goodbye!");
        }
        private static void Pause()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        //Menu Functions 
        private static string GetGreeting()
        {
            int hour = DateTime.Now.Hour;
            if (hour < 12) return "Good morning";
            if (hour < 18) return "Good afternoon";
            return "Good evening";
        }

        private void ShowStartupAlerts()
        {
            var expired = _fridge.ExpirationManager.CheckExpiredItems();
            var expiring = _fridge.ExpirationManager.GetExpiringItems();
            var dueReminders = _fridge.UserProfile.Reminders
                .Where(r => r.ReminderDate.Date <= DateTime.Today).ToList();

            if (expired.Count > 0)
                Console.WriteLine($"Warning: {expired.Count} item(s) have expired!");
            if (expiring.Count > 0)
                Console.WriteLine($"Heads up: {expiring.Count} item(s) expire soon.");
            if (dueReminders.Count > 0)
                Console.WriteLine($"You have {dueReminders.Count} reminder(s) due today.");
        }
        //Inventory Functions
        private void ShowInventory()
        {
            var items = _fridge.Items.Where(i => i.Quantity > 0).ToList();
            if (items.Count == 0)
            {
                Console.WriteLine("The Fridge is empty.");
                return;
            }
            Console.WriteLine($"{"Name",-15}{"Qty",5}{"Price",8}  Expires");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name,-15}" +
                                 $"{item.Quantity,5}" +
                                 $"{item.Price,8:0.00}  " +
                                 $"{item.ExpirationDate:dd/MM/yyyy}"); ;
                Console.WriteLine($"Total");
            }
            Console.WriteLine($"Total units: {_fridge.TotalQuantity}/{_fridge.MaxCapacity}");

        }
        private void AddItem()
        {
            Console.WriteLine("1. Food   2. Drink");
            int type = InputHelper.ReadInt("Type: ");

            if (type != 1 && type != 2)
            {
                Console.WriteLine("Invalid type.");
                return;
            }

            string name = InputHelper.ReadString("Name: ");
            int quantity = InputHelper.ReadPositiveInt("Quantity: ");
            double price = InputHelper.ReadPositiveDouble("Price: ");
            int days = InputHelper.ReadInt("Days until expiration: ");
            DateTime expiration = DateTime.Today.AddDays(days);

            if (type == 1)
            {
                var category = InputHelper.ReadEnum<FoodCategory>("Category");
                _fridge.AddItem(new FoodItem(name, quantity, price, expiration, category));
            }
            else
            {
                var category = InputHelper.ReadEnum<DrinkCategory>("Category");
                _fridge.AddItem(new DrinkItem(name, quantity, price, expiration, category));
            }

            Console.WriteLine($"Added {quantity} x {name}.");
        }
        private void TakeItemOut()
        {
            string name = InputHelper.ReadString("Item name: ");
            int quantity = InputHelper.ReadPositiveInt("Quantity to take out: ");
            _fridge.RemoveItem(name, quantity);
            Console.WriteLine($"Took out {quantity} x {name}.");
        }

        private void ReturnItem()
        {
            string name = InputHelper.ReadString("Item name: ");
            int quantity = InputHelper.ReadPositiveInt("Quantity to return: ");
            _fridge.ReturnItem(name, quantity);
            Console.WriteLine($"Returned {quantity} x {name}.");
        }
        //Expiration functions

        private void ShowExpiringItems()
        {
            var em = _fridge.ExpirationManager;
            var expired = em.CheckExpiredItems();
            var expiring = em.GetExpiringItems();

            Console.WriteLine("-- Expired --");
            if (expired.Count == 0) Console.WriteLine("None.");
            foreach (var item in expired)
                Console.WriteLine($"{item.Name} (expired {-em.DaysUntilExpiration(item)} day(s) ago)");

            Console.WriteLine($"-- Expiring within {em.ExpirationWarningDays} days --");
            if (expiring.Count == 0) Console.WriteLine("None.");
            foreach (var item in expiring)
                Console.WriteLine($"{item.Name} ({em.DaysUntilExpiration(item)} day(s) left)");
        }

        // ---------- Shopping list ----------

        private void BuildShoppingList()
        {
            var wanted = new Dictionary<string, int>();
            Console.WriteLine("Enter the items you want (empty name to finish):");

            while (true)
            {
                string name = InputHelper.ReadOptionalString("Item name: ");
                if (name == "") break;

                int quantity = InputHelper.ReadPositiveInt("Quantity wanted: ");
                if (wanted.ContainsKey(name)) wanted[name] += quantity;
                else wanted[name] = quantity;
            }

            ShoppingList list = _fridge.BuildShoppingList(wanted);
            if (list.Items.Count == 0)
            {
                Console.WriteLine("You already have everything!");
                return;
            }

            Console.WriteLine("You need to buy:");
            foreach (var entry in list.Items)
                Console.WriteLine($"{entry.Key} x{entry.Value}");
            Console.WriteLine($"Estimated cost: {list.CalculateTotalCost():0.00}");
        }
        // ---------- Reminders / notes / favorites ----------

        private void RemindersMenu()
        {
            Console.WriteLine("1. Show reminders   2. Add reminder   3. Remove reminder");
            int choice = InputHelper.ReadInt("Choose: ");
            var profile = _fridge.UserProfile;

            if (choice == 1)
            {
                if (profile.Reminders.Count == 0) Console.WriteLine("No reminders.");
                else profile.DisplayReminder();
            }
            else if (choice == 2)
            {
                string title = InputHelper.ReadString("Title: ");
                string description = InputHelper.ReadString("Description: ");
                int days = InputHelper.ReadInt("In how many days: ");
                string itemName = InputHelper.ReadOptionalString("Related item name (empty for none): ");
                Item? related = itemName == "" ? null : _fridge.Items.Find(i => i.Name == itemName);

                profile.AddReminder(new Reminder(title, description, DateTime.Today.AddDays(days), related));
                Console.WriteLine("Reminder added.");
            }
            else if (choice == 3)
            {
                string title = InputHelper.ReadString("Title of reminder to remove: ");
                Reminder? r = profile.Reminders.Find(x => x.Title == title);
                if (r == null)
                {
                    Console.WriteLine("Reminder not found.");
                }
                else
                {
                    profile.RemoveReminder(r);
                    Console.WriteLine("Removed.");
                }
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }

        private void NotesMenu()
        {
            Console.WriteLine("1. Add / update note   2. Read note");
            int choice = InputHelper.ReadInt("Choose: ");

            if (choice != 1 && choice != 2)
            {
                Console.WriteLine("Invalid option.");
                return;
            }

            string itemName = InputHelper.ReadString("Item name: ");

            if (choice == 1)
            {
                string note = InputHelper.ReadString("Note: ");
                _fridge.UserProfile.Notes[itemName] = note;
                Console.WriteLine("Note saved.");
            }
            else
            {
                Console.WriteLine(_fridge.UserProfile.GetNote(itemName));
            }
        }

        private void FavoritesMenu()
        {
            Console.WriteLine("1. Show favorites   2. Add favorite from fridge   3. Remove favorite");
            int choice = InputHelper.ReadInt("Choose: ");
            var profile = _fridge.UserProfile;

            if (choice == 1)
            {
                if (profile.FavoriteItems.Count == 0) Console.WriteLine("No favorites yet.");
                else profile.DisplayFavorites();
            }
            else if (choice == 2)
            {
                string name = InputHelper.ReadString("Item name: ");
                Item? item = _fridge.Items.Find(i => i.Name == name);

                if (item == null) Console.WriteLine("Item not found in fridge.");
                else if (profile.FavoriteItems.Contains(item)) Console.WriteLine("Already a favorite.");
                else
                {
                    profile.AddFavorite(item);
                    Console.WriteLine("Added to favorites.");
                }
            }
            else if (choice == 3)
            {
                string name = InputHelper.ReadString("Item name: ");
                Item? item = profile.FavoriteItems.Find(i => i.Name == name);

                if (item == null)
                {
                    Console.WriteLine("Not in favorites.");
                }
                else
                {
                    profile.RemoveFavorite(item);
                    Console.WriteLine("Removed.");
                }
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }

            }
        }
    }