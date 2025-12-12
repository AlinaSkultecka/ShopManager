using ShopManager.Data;
using ShopManager.ProductFolder;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopManager.Helpers
{
    public static class ProductAdminHelper
    {
        public static async Task ProductAdminMenu(MongoDbService db)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Product admin ===");
                Console.WriteLine("1. List products");
                Console.WriteLine("2. Add product");
                Console.WriteLine("3. Edit product (price/stock)");
                Console.WriteLine("4. Delete product");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ListProducts(db);
                        break;

                    case "2":
                        await AddProduct(db);
                        break;

                    case "3":
                        await EditProduct(db);
                        break;

                    case "4":
                        await DeleteProduct(db);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid choice");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task ListProducts(MongoDbService db)
        {
            Console.Clear();
            var products = await db.GetAllProductsAsync();

            if (!products.Any())
            {
                Console.WriteLine("No products found.");
            }
            else
            {
                Console.WriteLine("=== Product list ===");
                foreach (var p in products.OrderBy(p => p.ProductCategory).ThenBy(p => p.Id))
                {
                    Console.WriteLine($"ID: {p.Id} | {p.ProductCategory} | {p.Name} | Price: {p.Price} | Stock: {p.Stock}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task AddProduct(MongoDbService db)
        {
            Console.Clear();
            Console.WriteLine("=== Add product ===");

            Console.Write("Category: ");
            string category = Console.ReadLine() ?? "";

            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid price.");
                Console.ReadKey();
                return;
            }

            Console.Write("Stock: ");
            if (!int.TryParse(Console.ReadLine(), out int stock))
            {
                Console.WriteLine("Invalid stock.");
                Console.ReadKey();
                return;
            }

            // Generate a new int Id using IdGenerator (which has been synced from DB)
            int newId = IdGenerator.NextId();

            var product = new ProductProperties(newId, category, name, price, stock);

            await db.CreateProductAsync(product);

            Console.WriteLine("Product added.");
            Console.ReadKey();
        }

        private static async Task EditProduct(MongoDbService db)
        {
            Console.Clear();
            Console.WriteLine("=== Edit product ===");

            var products = await db.GetAllProductsAsync();

            if (!products.Any())
            {
                Console.WriteLine("No products to edit.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter product ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid id.");
                Console.ReadKey();
                return;
            }

            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Editing {product.Name} (current price: {product.Price}, stock: {product.Stock})");

            Console.Write("New price (leave empty to keep): ");
            string priceInput = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal newPrice))
            {
                product.Price = newPrice;
            }

            Console.Write("New stock (leave empty to keep): ");
            string stockInput = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(stockInput) && int.TryParse(stockInput, out int newStock))
            {
                product.Stock = newStock;
            }

            await db.UpdateProductAsync(product);

            Console.WriteLine("Product updated.");
            Console.ReadKey();
        }

        private static async Task DeleteProduct(MongoDbService db)
        {
            Console.Clear();
            Console.WriteLine("=== Delete product ===");

            var products = await db.GetAllProductsAsync();
            if (!products.Any())
            {
                Console.WriteLine("No products to delete.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter product ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid id.");
                Console.ReadKey();
                return;
            }

            // Check if the product with this Id actually exists
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                Console.WriteLine("Product with this ID does not exist. No deletion performed.");
                Console.ReadKey();
                return;
            }

            // The product exists – OK to delete
            await db.DeleteProductAsync(id);

            Console.WriteLine($"Product '{product.Name}' (ID: {product.Id}) deleted.");
            Console.ReadKey();
        }
    }
}

