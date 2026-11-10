using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridge
{
    internal class UserProfile
    {
        public string Username;
        public string Diet;
        public List<Item> FavoriteItems;
        public List<Reminder> Reminders;
        public Dictionary<string, string> Notes;


    }
}
