using ShopManager.Customer;
using ShopManager.Product;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager
{
    public class ShopHelper
    {
        // Case 1: Choose a product
        public static void ShowProductInfo()
        {
            Console.Clear();
            Console.WriteLine("PRODUCT LIST");
            var grouped = ProductsList.AllProducts.GroupBy(p => p.ProductCategory);

            foreach (var group in grouped)
            {
                Console.WriteLine($"\n=== {group.Key} ===");
                foreach (var product in group)
                {
                    CultureInfo swedish = new CultureInfo("sv-SE");
                    Console.WriteLine($"ID: {product.Id} | Name: {product.Name} | Price: {product.Price.ToString("C", swedish)} | Stock: {product.Stock}");
                }
            }
        }

        public static void AddToShoppingCart(Customer.Customer user)
        {
            ShowProductInfo();
            Console.WriteLine("\nType 'Done' anytime to finish adding products.");

            while (true)
            {
                // Get product ID
                Console.Write("\nEnter the ID of the product you want to buy: ");
                string inputId = Console.ReadLine()!.ToLower();

                if (inputId == "done")
                    return; // exit to main menu

                if (!int.TryParse(inputId, out int productId))
                {
                    Console.WriteLine("Invalid input! Please enter a product ID or 'Done'.");
                    continue;
                }

                //Find product
                Product.ProductDetails product = null;
                foreach (var p in Product.ProductsList.AllProducts)
                {
                    if (p.Id == productId)
                    {
                        product = p;
                        break;
                    }
                }
                if (product == null)
                {
                    Console.WriteLine("Product not found.");
                    continue;
                }

                //Get quantity
                Console.Write("Enter quantity (available stock: " + product.Stock + "): ");
                string inputQty = Console.ReadLine()!.ToLower();

                if (inputQty == "done")
                    return; // exit to main menu

                if (!int.TryParse(inputQty, out int quantity))
                {
                    Console.WriteLine("Invalid input! Please enter a number or 'Done'.");
                    continue;
                }

                if (quantity <= 0)
                {
                    Console.WriteLine("Quantity must be more than 0.");
                    continue;
                }

                if (quantity > product.Stock)
                {
                    Console.WriteLine("Not enough stock available.");
                    continue;
                }

                //Add to cart
                user.ShoppingCart.Add(new ShoppingCartItem(product, quantity));
                product.Stock -= quantity; // decrease stock

                Console.WriteLine($"{quantity} x {product.Name} added to your shopping cart.");

            }
        }

        // Case 2: Shopping cart
        public static void ViewShoppingCart(Customer.Customer user)
        {
            Console.Clear();
            CultureInfo swedish = new CultureInfo("sv-SE");
            if (user.ShoppingCart.Count == 0)
            {

                Console.WriteLine(user.ToString());
                Console.WriteLine("Your cart is empty!");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine(user.ToString());
                Console.WriteLine("\n=== Your Shopping Cart ===");
                foreach (var item in user.ShoppingCart)
                {
                    Console.WriteLine($"{item.Product.Name} | Qty: {item.Quantity} | Price: {item.Product.Price.ToString("C", swedish)} | Total: {item.TotalPrice.ToString("C", swedish)}");
                }

                decimal grandTotal = user.ShoppingCart.Sum(c => c.TotalPrice);
                Console.WriteLine($"Grand Total: {grandTotal.ToString("C", swedish)}");

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }

        // Case 3: Make payment
        public static void MakePayment(Customer.Customer user)
        {
            Console.Clear();
            if (user.ShoppingCart.Count == 0)
            {
                Console.WriteLine("Your cart is empty!");
                Console.ReadKey();
                return;
            }
            
            decimal grandTotal = 0;
            CultureInfo swedish = new CultureInfo("sv-SE");
            foreach (var item in user.ShoppingCart)
            {
                grandTotal += item.TotalPrice;
            }

            Console.WriteLine($"Your total price without a discount is {grandTotal.ToString("C", swedish)}.\n");
            Discount.ShowUserDiscount(user, grandTotal);
            Console.WriteLine("Proceed to payment? (yes/no):");
            string choice = Console.ReadLine()!.ToLower();
            if (choice == "yes")
            {
                // Simulate payment processing
                Console.WriteLine("\nProcessing payment...");
                System.Threading.Thread.Sleep(1500); // Simulate delay
                Console.WriteLine("\nPayment successful! Thank you for your purchase.");
                // Clear cart after payment
                user.ShoppingCart.Clear();
            }
            else
            {
                Console.WriteLine("Payment cancelled.");
            }
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

    }
}
