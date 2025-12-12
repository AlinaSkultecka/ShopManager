# 🧸 Toy Shop Console Application

ShopManager is a **console-based toy shop application** built with C#.  
It uses **MongoDB Atlas** for data storage and demonstrates:

- user registration & login  
- membership levels and discounts  
- shopping cart logic  
- admin-only product management (CRUD) against a MongoDB database.


### 🧱 Tech Stack

- C# / .NET (console app)
- MongoDB Atlas
- MongoDB .NET/C# Driver


### ⚙️ MongoDB Atlas Connection String Handling

> 🔐 The MongoDB connection string is **not** stored in the source code and is **not** committed to GitHub.

The app reads the connection string from a **local text file**:

- Relative path at runtime:  
  `Data/mongo-connection.txt`

**`MongoDbService`** looks for the file like this:

```csharp
var basePath = AppContext.BaseDirectory;
var path = Path.Combine(basePath, "Data", "mongo-connection.txt");
// file must contain a single line: MongoDB Atlas connection string
var connectionString = File.ReadAllText(path);


