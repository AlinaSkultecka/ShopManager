using MongoDB.Driver;
using ShopManager.CustomerFolder;
using ShopManager.ProductFolder;

namespace ShopManager.Data
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _db;

        public MongoDbService()
        {
            // Connect to MongoDB Atlas
            // Look for the file in the same folder as the .exe
            var basePath = AppContext.BaseDirectory;
            var path = Path.Combine(basePath, "Data", "mongo-connection.txt");

            if (!File.Exists(path))
            {
                throw new InvalidOperationException(
                    $"Connection string file '{path}' not found. " +
                    "Create it and put your MongoDB Atlas connection string inside.");
            }

            var connectionString = File.ReadAllText(path).Trim();

            var clientSettings = MongoClientSettings.FromConnectionString(connectionString);

            // Optional but recommended (Server API version V1)
            clientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
            
            // Create MongoDB client
            var client = new MongoClient(clientSettings);  // client is the object your app uses to talk to the MongoDB Atlas cluster

            // Database name
            _db = client.GetDatabase("ToyShopDb");
        }

       
        
        // Collections
        public IMongoCollection<CustomerProperties> Customers
            => _db.GetCollection<CustomerProperties>("Customers");

        public IMongoCollection<ProductProperties> Products
            => _db.GetCollection<ProductProperties>("Products");

        
        
        // === Customers CRUD ===
        // Create
        public async Task CreateCustomerAsync(CustomerProperties customer)
        {
            await Customers.InsertOneAsync(customer);
        }

        // Read
        public async Task<List<CustomerProperties>> GetAllCustomersAsync()
        {
            return await Customers.Find(_ => true).ToListAsync();
        }

        // Update
        public async Task UpdateCustomerAsync(CustomerProperties customer)
        {
            var filter = Builders<CustomerProperties>.Filter.Eq(c => c.Username, customer.Username);
            await Customers.ReplaceOneAsync(filter, customer);
        }

        // Delete
        public async Task DeleteCustomerAsync(string username)
        {
            var filter = Builders<CustomerProperties>.Filter.Eq(c => c.Username, username);
            await Customers.DeleteOneAsync(filter);
        }

        
        
        
        // === Products CRUD ===
        // Create
        public async Task CreateProductAsync(ProductProperties product)
        {
            await Products.InsertOneAsync(product);
        }

        // Read
        public async Task<List<ProductProperties>> GetAllProductsAsync()
        {
            return await Products.Find(_ => true).ToListAsync();
        }

        // Update
        public async Task UpdateProductAsync(ProductProperties product)
        {
            var filter = Builders<ProductProperties>.Filter.Eq(p => p.Id, product.Id);
            await Products.ReplaceOneAsync(filter, product);
        }

        // Delete
        public async Task DeleteProductAsync(int productId)
        {
            var filter = Builders<ProductProperties>.Filter.Eq(p => p.Id, productId);
            await Products.DeleteOneAsync(filter);
        }
    }
}

