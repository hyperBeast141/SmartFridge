using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridge
{
    internal class Reminder
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReminderDate { get; set; }
        public Item? RelatedItem { get; set; }

        public Reminder(string title, string description, DateTime reminderDate, Item? relatedItem)
        {
            Title = title;
            Description = description;
            ReminderDate = reminderDate;
            RelatedItem = relatedItem;
        }
    }
}
