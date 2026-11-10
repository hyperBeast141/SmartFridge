using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridge
{
    internal class ExpirationManager
    {
        public List<Item> Items;
        public int ExpirationWarningDays;//כמה ימים לפי שפג התוקף להזהיר את המשתמש

        public ExpirationManager()
        {
            Items = new List<Item>();
            ExpirationWarningDays = 3;
        }
        public bool IsExpired(Item item)//בודק אם פג התוקף
        {
            return item.ExpirationDate < DateTime.Today;
        }
        public int DaysUntilExpiration(Item item)//מחזיר כמה ימים נשאר 
        {
            return (item.ExpirationDate.Date - DateTime.Today).Days;
        }
        public List<Item> CheckExpiredItems()//מחזיר את מה שפג לו התוקף
        {
            return Items.Where(item => IsExpired(item)).ToList();
        }
        public List<Item> GetExpiringItems()//מחזיר מה פג בקרוב
        {
            return Items.Where(item =>
                !IsExpired(item) &&
                DaysUntilExpiration(item) <= ExpirationWarningDays
            ).ToList();
        }


    }
}
