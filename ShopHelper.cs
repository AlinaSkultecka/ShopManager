using ShopManager.CustomerFolder;
using ShopManager.ProductFolder;
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
            var grouped = ProductList.AllProducts.GroupBy(p => p.ProductCategory);  //LINQ, some kind of magic

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

        public static void AddToShoppingCart(CustomerFolder.CustomerProperties user)
        {
            while (true)
            {
                // 1.Show product list
                ShowProductInfo();
                Console.WriteLine("\nType 'Done' anytime to finish adding products.");

                // Get product ID
                Console.Write("\nEnter the ID of the product you want to buy: ");
                string inputId = (Console.ReadLine() ?? "").ToLower();

                if (inputId == "done")
                    break;

                if (!int.TryParse(inputId, out int productId))
                {
                    Console.WriteLine("Invalid input! Please enter a product ID or 'Done'.");
                    Console.ReadKey(true);
                    continue;
                }

                
                // 2.Find product
                ProductProperties product = null;
                foreach (var p in ProductFolder.ProductList.AllProducts)
                {
                    if (p.Id == productId)
                    {
                        product = p; // the product was found
                        break;       // stop searching
                    }
                }

                if (product == null)
                {
                    Console.WriteLine("Product not found.");
                    Console.ReadKey(true); 
                    continue;              // go to the next loop iteration
                }

                // Get quantity
                Console.Write($"Enter quantity (available stock: {product.Stock}): ");
                string inputQty = (Console.ReadLine() ?? "").ToLower();

                if (inputQty == "done")
                    break;

                if (!int.TryParse(inputQty, out int quantity))
                {
                    Console.WriteLine("Invalid input! Please enter a number or 'Done'.");
                    Console.ReadKey(true);
                    continue;
                }

                if (quantity <= 0)
                {
                    Console.WriteLine("Quantity must be more than 0.");
                    Console.ReadKey(true);
                    continue;
                }

                if (quantity > product.Stock)
                {
                    Console.WriteLine("Not enough stock available.");
                    Console.ReadKey(true);
                    continue;
                }

                
                // 3.Add to cart
                user.ShoppingCart.Add(new ShoppingCartItem(product, quantity));
                product.Stock -= quantity; // decrease stock

                Console.WriteLine($"\n{quantity} x {product.Name} added to your shopping cart.\n");
                Console.ReadKey(true);
            }
        }

        // Case 2: Shopping cart
        public static void ViewShoppingCart(CustomerFolder.CustomerProperties user)
        {
            //Console.Clear();
            CultureInfo swedish = new CultureInfo("sv-SE");

            if (user.ShoppingCart.Count == 0)
            {
                Console.WriteLine("Your cart is empty!");
                Console.ReadKey();
                return;
            }

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Your Shopping Cart ===");

                decimal grandTotal = 0;
                foreach (var item in user.ShoppingCart)
                {
                    Console.WriteLine($"ID: {item.Product.Id} | {item.Product.Name} | Qty: {item.Quantity} | Price: {item.Product.Price.ToString("C", swedish)} | Total: {item.TotalPrice.ToString("C", swedish)}");
                    grandTotal += item.TotalPrice;
                }

                Console.WriteLine($"Grand Total: {grandTotal.ToString("C", swedish)}");

                DisplayShoppingCartMenu();

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        RemoveItemFromCart(user);

                        // If cart becomes empty, exit loop
                        if (user.ShoppingCart.Count == 0)
                        {
                            Console.WriteLine("\nYour cart is now empty.");
                            Console.WriteLine("Press any key to return to the menu...");
                            Console.ReadKey();
                            running = false;
                        }
                        break;

                    case "0":
                        running = false; // exit
                        break;

                    default:
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void DisplayShoppingCartMenu()
        {
            Console.WriteLine("\nChoose what you want to do:");
            Console.WriteLine("1. Remove an item");
            Console.WriteLine("0. Go back to menu");
            Console.Write("Enter your choice: ");
        }

        private static void RemoveItemFromCart(CustomerFolder.CustomerProperties user)
        {
            Console.Write("Enter the ID of the product to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int productID))
            {
                Console.WriteLine("Invalid input! Please enter a number.");
                return;
            }

            ShoppingCartItem? itemToRemove = null;

            // Find the item in the shopping cart
            foreach (var item in user.ShoppingCart)
            {
                if (item.Product.Id == productID)
                {
                    itemToRemove = item;
                    break; // stop searching once found
                }
            }

            if (itemToRemove != null)
            {
                Console.Write($"Enter quantity to remove (max {itemToRemove.Quantity}): ");
                if (!int.TryParse(Console.ReadLine(), out int qtyToRemove) || qtyToRemove <= 0)
                {
                    Console.WriteLine("Invalid quantity!");
                    return;
                }

                if (qtyToRemove >= itemToRemove.Quantity)
                {
                    // Remove the entire item
                    itemToRemove.Product.Stock += itemToRemove.Quantity;
                    user.ShoppingCart.Remove(itemToRemove);
                    Console.WriteLine($"{itemToRemove.Product.Name} completely removed from your cart.");
                }
                else
                {
                    // Remove part of the quantity
                    itemToRemove.Quantity -= qtyToRemove;
                    itemToRemove.Product.Stock += qtyToRemove;
                    Console.WriteLine($"{qtyToRemove} of {itemToRemove.Product.Name} removed from your cart. Remaining: {itemToRemove.Quantity}");
                }
            }
            else
            {
                Console.WriteLine("Item not found in your cart.");
            }
        }


        // Case 3: Make payment
        public static void MakePayment(CustomerFolder.CustomerProperties user)
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
                Discount.UpgradeMembership(user, grandTotal);
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
