# 🧸 Toy Shop Console Application

ShopManager is a **console-based toy shop application** built with C#.  
It uses **MongoDB Atlas** for data storage and demonstrates:

- user registration & login  
- membership levels and discounts  
- shopping cart logic  
- admin-only product management (CRUD) against a MongoDB database.


## ✨ Features

### 👤 User Management

- Register a new account  
- Log in with username and password  
- Track user membership levels:
  - **BronseLevel** 🥉
  - **SilverLevel** 🥈
  - **GoldLevel** 🥇  
- Membership is automatically upgraded based on total purchases.


### 🛍 Product Management

- Products are stored in **MongoDB Atlas** in the `Products` collection.
- Products are **categorized** (SoftToy, Lego, BabyToy, BoardGame, etc.).
- Admin user can:
  - List all products
  - Add new products (with unique integer `Id`)
  - Edit price and stock
  - Delete products (only if the product exists)


### 🛒 Shopping Cart

- Logged-in customers can:
  - Browse products
  - Add items to their cart (with stock validation)
  - Remove part or all of the quantity of a product in the cart
- Cart shows:
  - Product name
  - Quantity
  - Unit price
  - Total per item
  - Grand total

Cart lives in memory per logged-in user (not persisted between sessions).


### 💳 Payments & Discounts

- Calculates **grand total** of the cart.
- Applies **discounts** based on customer level.
- After a successful payment:
  - Payment is simulated with a short delay.
  - Membership level can be upgraded (e.g. Bronze → Silver → Gold).
  - Cart is cleared.


### 🧾 Data Persistence (MongoDB Atlas)

- Uses **MongoDB Atlas** as the database backend.
- Database name: `ToyShopDb`
- Collections:
  - `Customers`
  - `Products`
- On first run (when collections are empty), the app:
  - Seeds default **customers**:
    - `aa / 11` (admin, BronseLevel)
    - `Knatte / 123`
    - `Fnatte / 321`
    - `Tjatte / 213`
  - Seeds default **products** from a static `ProductList` (soft toys, Lego, baby toys, board games).
- Product stock changes (add/remove from cart, admin edits) are written back to MongoDB.


## 👮‍♂️ User Roles

There is a simple “admin vs normal user” behaviour:

### Admin User

- **Username:** `aa`  
- **Password:** `11`  

When logged in as this user:

- The menu shows:
  - `1. Manage products`
  - `2. Logout`
  - `0. Exit`
- Admin **cannot**:
  - choose products
  - view cart
  - pay
- Admin **can only** manage products (full CRUD).

### Normal Users

For any other user:

- The menu shows:
  - `1. Choose a product`
  - `2. Shopping cart`
  - `3. Pay`
  - `4. Logout`
  - `0. Exit`
- Normal users **cannot** access product management.


## 🧱 Tech Stack

- C# / .NET (console app)
- MongoDB Atlas
- MongoDB .NET/C# Driver
- Basic console UI (no GUI framework)


## ⚙️ MongoDB Atlas Connection String Handling

> 🔐 The MongoDB connection string is **not** stored in the source code and is **not** committed to GitHub.

The app reads the connection string from a **local text file**:

- Relative path at runtime:  
  `Data/mongo-connection.txt`

**`MongoDbService`** looks for the file like this:

```csharp
var basePath = AppContext.BaseDirectory;
var path = Path.Combine(basePath, "Data", "mongo-connection.txt");
// file must contain a single line: MongoDB Atlas connection string
var connectionString = File.ReadAllText(path).Trim();


