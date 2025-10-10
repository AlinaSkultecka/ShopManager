using ShopManager.CustomerFolder;
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
        public static void ShowUserDiscount(CustomerFolder.CustomerProperties currentUser, decimal grandTotal)
        {
            CultureInfo swedish = new CultureInfo("sv-SE");
            switch (currentUser.Level)
            {
                case ShopManager.CustomerFolder.CustomerLevel.BronseLevel:
                    Console.WriteLine("You are at Bronse Level, your member discount is 5%.");
                    decimal grandTotalDiscountedBronse = grandTotal * 0.95m;
                    Console.WriteLine($"\nYour total price after the discount is applied is {grandTotalDiscountedBronse.ToString("C", swedish)}.");
                    break;
                case ShopManager.CustomerFolder.CustomerLevel.SilverLevel:
                    Console.WriteLine("You are at Silver Level, your member discount is 10%.");
                    decimal grandTotalDiscountedSilver = grandTotal * 0.90m;
                    Console.WriteLine($"\nYour total price after the discount is applied is {grandTotalDiscountedSilver.ToString("C", swedish)}.");
                    break;
                case ShopManager.CustomerFolder.CustomerLevel.GoldLevel:
                    Console.WriteLine("You are at Gold Level, your member discount is 15%.");
                    decimal grandTotalDiscountedGold = grandTotal * 0.85m;
                    Console.WriteLine($"\nYour total price after the discount is applied is {grandTotalDiscountedGold.ToString("C", swedish)}.");
                    break;
            }
        }

        // Upgrade membership level 
        public static void UpgradeMembership(CustomerFolder.CustomerProperties currentUser, decimal grandTotal)
        {
            if (grandTotal >= 1000)
            {
                CustomerLevel previousLevel = currentUser.Level;

                // Upgrade level
                if (currentUser.Level == CustomerLevel.BronseLevel)
                    currentUser.SetLevel(CustomerLevel.SilverLevel);
                else if (currentUser.Level == CustomerLevel.SilverLevel)
                    currentUser.SetLevel(CustomerLevel.GoldLevel);

                // Notify user
                if (previousLevel != currentUser.Level)
                    Console.WriteLine($"Congrats! Your membership is now {currentUser.Level}!");
            }
        }
    }
}
