using ShopManager.CustomerFolder;
using ShopManager.ProductFolder;
using System.Linq;
using System.Media;
using System;
using System.Threading.Tasks;
using NAudio.Wave;


namespace ShopManager
{
    internal class Program
    {
        private static List<CustomerFolder.CustomerProperties> users = new List<CustomerFolder.CustomerProperties>();
        private static CustomerFolder.CustomerProperties? currentUser = null;
        public List<ShoppingCartItem> ShoppingCart = new List<ShoppingCartItem>();

        
        // Simple user interface in the console
        static void Main(string[] args)
        {
            // Add default/mock customers
            var mock = new CustomerFolder.CustomerConstructor();
            users.AddRange(mock.Customers);

            while (true)
            {
                if (currentUser == null)
                {
                    // Login in or register
                    DisplayMenu();
                    string choice = Console.ReadLine() ?? "";
                    switch (choice)
                    {
                        case "1": Register(); break;
                        case "2": Login(); break;
                        case "0": return;
                        default: Console.WriteLine("Invalid option"); break;
                    }
                }
                else
                {
                    // Music
                    string filePath = "C:\\Users\\grigo\\Desktop\\C#\\3. Inlämning upgifter\\ShopManager\\music_for_app.wav";

                    using var audioFile = new AudioFileReader(filePath);
                    using var outputDevice = new WaveOutEvent();
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    // User is logged in
                    DisplayUserMenu();
                    string choice = Console.ReadLine() ?? "";
                    switch (choice)
                    {
                        case "1": ShopHelper.AddToShoppingCart(currentUser); break;
                        case "2": ShopHelper.ViewShoppingCart(currentUser); break;
                        case "3": ShopHelper.MakePayment(currentUser); break; 
                        case "4": currentUser = null; Console.WriteLine("Logged out."); break;
                        case "0": return;
                        default: Console.WriteLine("Invalid option"); break;
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

            Console.WriteLine($"=== Hello {currentUser!.Username}, {currentUser!.Level} customer ===");
            Console.WriteLine("1. Choose a product");
            Console.WriteLine("2. Shopping cart");
            Console.WriteLine("3. Pay");
            Console.WriteLine("4. Logout");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
        }

        private static void Register()
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
                    return;
                }
            }

            // Create and add the new user
            var user = new CustomerFolder.CustomerProperties(username, password);
            users.Add(user);

            // Add new customer to the text file
            string filePath = "C:\\Users\\grigo\\Desktop\\C#\\3. Inlämning upgifter\\ShopManager\\CustomerFolder\\MockCustomers.txt";
            using (StreamWriter sw = new StreamWriter(filePath, true)) // 'true' for adding to the file
            {
                sw.WriteLine(user.ToString()); // ToString() = "username,password,Level"
            }
            Console.WriteLine("Registration successful!\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("=== User Login ====");
            Console.Write("Enter username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? "";

            bool loggedIn = false;

            foreach (CustomerFolder.CustomerProperties u in users)
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
    }
}

