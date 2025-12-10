using ShopManager.CustomerFolder;
using ShopManager.ProductFolder;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using ShopManager.Data;
using NAudio.Wave;
using ShopManager.Helpers;


namespace ShopManager
{
    internal class Program
    {
        private static List<CustomerFolder.CustomerProperties> users = new List<CustomerFolder.CustomerProperties>();
        private static CustomerFolder.CustomerProperties? currentUser = null;

        private static MongoDbService db = new MongoDbService();   // Atlas connection
        public List<ShoppingCartItem> ShoppingCart = new List<ShoppingCartItem>();

        
        // Simple user interface in the console
        static async Task Main(string[] args)
        {
            // Mock customers
            // Load customers from MongoDB Atlas
            users = await db.GetAllCustomersAsync();

            // If there are no customers yet, seed some defaults
            if (!users.Any())
            {
                await SeedDefaultCustomers();
            }

            // Products
            await SeedProductsIfEmpty();

            // Sync IdGenerator with the current products in Mongo
            await SyncProductIdGenerator();

            while (true)
            {
                if (currentUser == null)
                {
                    // Login in or register
                    DisplayMenu();
                    string choice = Console.ReadLine() ?? "";
                    switch (choice)
                    {
                        case "1": await Register(); break;
                        case "2": Login(); break;
                        case "0": return;
                        default:
                            Console.WriteLine("Invalid option");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    // User is logged in
                    DisplayUserMenu();
                    string choice = Console.ReadLine() ?? "";

                    if (IsAdmin(currentUser!))
                    {
                        // === ADMIN ACTIONS ONLY ===
                        switch (choice)
                        {
                            case "1":
                                await ProductAdminHelper.ProductAdminMenu(db);   // only admin can manage products
                                break;

                            case "2":
                                currentUser = null;
                                Console.WriteLine("Logged out.");
                                Console.ReadKey();
                                break;

                            case "0":
                                return;

                            default:
                                Console.WriteLine("Invalid option");
                                Console.ReadKey();
                                break;
                        }
                    }
                    else
                    {
                        // === NORMAL CUSTOMER ACTIONS ===
                        switch (choice)
                        {
                            case "1":
                                await ShopHelper.AddToShoppingCart(currentUser!, db);
                                break;

                            case "2":
                                await ShopHelper.ViewShoppingCart(currentUser!, db);
                                break;

                            case "3":
                                ShopHelper.MakePayment(currentUser!, db);
                                break;

                            case "4":
                                currentUser = null;
                                Console.WriteLine("Logged out.");
                                Console.ReadKey();
                                break;

                            case "0":
                                return;

                            default:
                                Console.WriteLine("Invalid option");
                                Console.ReadKey();
                                break;
                        }
                    }
                }
            }
        }    

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Welcome to Toy Shop ===\n");
            Console.WriteLine("To continue login in to your account"); 
            Console.WriteLine("(or register if you do not have one)");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
        }

        private static void DisplayUserMenu()
        {
            Console.Clear();

            if (currentUser != null && IsAdmin(currentUser))
            {
                // Admin menu
                Console.WriteLine($"=== Hello Admin ===");
                Console.WriteLine("1. Manage products");
                Console.WriteLine("2. Logout");
                Console.WriteLine("0. Exit");
            }
            else
            {
                // Normal customer menu
                Console.WriteLine($"=== Hello {currentUser!.Username}, {currentUser!.Level} customer ===");
                Console.WriteLine("1. Choose a product");
                Console.WriteLine("2. Shopping cart");
                Console.WriteLine("3. Pay");
                Console.WriteLine("4. Logout");
                Console.WriteLine("0. Exit");
            }

            Console.Write("Enter your choice: ");
        }

        private static async Task Register()
        {
            Console.Clear();
            Console.WriteLine("=== User Registration ====");
            Console.Write("Enter username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? "";

            // Check if username already exists
            foreach (CustomerFolder.CustomerProperties u in users)
            {
                if (u.Username == username)
                {
                    Console.WriteLine("Username already exists!\n");
                    Console.ReadKey();
                    return;
                }
            }

            // Create and add the new user
            var user = new CustomerFolder.CustomerProperties(username, password);
            
            // Save to MongoDB Atlas
            await db.CreateCustomerAsync(user);

            // Add to in-memory list
            users.Add(user);

            Console.WriteLine("Registration successful!\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("Only Admin has the right to manage products (add/list/update/delete)");
            Console.WriteLine("Admin's username: aa, password:11\n");
            Console.WriteLine("=== User Login ====");
            Console.Write("Enter username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? "";

            bool loggedIn = false;

            foreach (CustomerProperties u in users)
            {
                if (u.Username == username && u.Password == password)
                {
                    currentUser = u;  // set the logged-in user
                    loggedIn = true;
                    break;  // stop checking after the user is found
                }
            }

            if (loggedIn)
            {
                Console.WriteLine($"Welcome back, {currentUser.Username}!");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid credentials!");
                Console.ReadKey();
            }
        }


        // === SEED HELPERS ===
        private static async Task SeedDefaultCustomers()
        {
            var defaultCustomers = new List<CustomerProperties>
            {
                new CustomerProperties("aa", "11", CustomerLevel.BronseLevel),
                new CustomerProperties("Knatte", "123", CustomerLevel.BronseLevel),
                new CustomerProperties("Fnatte", "321", CustomerLevel.BronseLevel),
                new CustomerProperties("Tjatte", "213", CustomerLevel.BronseLevel)
            };

            foreach (var c in defaultCustomers)
            {
                await db.CreateCustomerAsync(c);   // save to Atlas
                users.Add(c);           // keep in memory
            }
        }

        private static async Task SeedProductsIfEmpty()
        {
            // Wait for MongoDB to return the list
            var productsInDb = await db.GetAllProductsAsync();

            if (productsInDb.Any())
                return; // already seeded

            // Use your existing static product list
            var allProducts = ProductFolder.ProductList.AllProducts;

            foreach (var p in allProducts)
            {
                await db.CreateProductAsync(p);
            }
        }

        private static async Task SyncProductIdGenerator()
        {
            var products = await db.GetAllProductsAsync();

            if (products.Any())
            {
                int maxId = products.Max(p => p.Id);
                ProductFolder.IdGenerator.Initialize(maxId);
            }
            else
            {
                // No products at all, start from 0
                ProductFolder.IdGenerator.Initialize(0);
            }
        }

        private static bool IsAdmin(CustomerProperties user)
        {
            // Admin is your seeded user aa / 11
            return user.Username == "aa" && user.Password == "11";
        }
    }
}

