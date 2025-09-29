using ShopManager.Customer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager
{
    public class Discount
    {
        public static void ShowUserDiscount(Customer.Customer currentUser, decimal grandTotal)
        {
            CultureInfo swedish = new CultureInfo("sv-SE");
            switch (currentUser.Level)
            {
                case ShopManager.Customer.UserLevel.BronseLevel:
                    Console.WriteLine("You are at Bronse Level, your member discount is 5%.");
                    decimal grandTotalDiscountedBronse = grandTotal * 0.95m;
                    Console.WriteLine($"\nYour total after discount is {grandTotalDiscountedBronse.ToString("C", swedish)}.");
                    break;
                case ShopManager.Customer.UserLevel.SilverLevel:
                    Console.WriteLine("You are at Silver Level, your member discount is 10%.");
                    decimal grandTotalDiscountedSilver = grandTotal * 0.90m;
                    Console.WriteLine($"\nYour total after discount is {grandTotalDiscountedSilver.ToString("C", swedish)}.");
                    break;
                case ShopManager.Customer.UserLevel.GoldLevel:
                    Console.WriteLine("You are at Gold Level, your member discount is 15%.");
                    decimal grandTotalDiscountedGold = grandTotal * 0.85m;
                    Console.WriteLine($"\nYour total after discount is {grandTotalDiscountedGold.ToString("C", swedish)}.");
                    break;
            }
        }

        // Upgrade membership level 
        public static void UpgradeMembership(Customer.Customer currentUser, decimal grandTotal)
        {
            if (grandTotal >= 1000)
            {
                UserLevel previousLevel = currentUser.Level;

                // Upgrade level
                if (currentUser.Level == UserLevel.BronseLevel)
                    currentUser.SetLevel(UserLevel.SilverLevel);
                else if (currentUser.Level == UserLevel.SilverLevel)
                    currentUser.SetLevel(UserLevel.GoldLevel);

                // Notify user
                if (previousLevel != currentUser.Level)
                    Console.WriteLine($"Congrats! Your membership is now {currentUser.Level}!");
            }
        }
    }
}
