# Toy Shop Console Application

## Description

This is a **console-based toy shop application** written in C#.  
It allows users to:

- Register new accounts or log in with existing accounts (including preloaded default users).  
- Browse available products (Soft Toys, Lego, Baby Toys, Board Games).  
- Add products to a shopping cart.  
- View the shopping cart with a total price in **Swedish Krona (SEK)**.  
- Logout and switch accounts.  

///**Note:** Shopping carts are **not saved between runs**, but registered users are saved in a text file (`users.txt`) for persistence.

---

## Features

1. **User Management**  
   - Register new users.  
   - Login with existing users (case-insensitive usernames).  
   - Preloaded mock users:  
     - `Knatte` / `123`  
     - `Fnatte` / `321`  
     - `Tjatte` / `213`  

2. **Product Catalog**  
   - Browse products grouped by category.  
   - See prices in Swedish Krona (`kr`) and available stock.  

3. **Shopping Cart**  
   - Add products to cart.  
   - View cart contents with quantities, individual prices, and grand total in SEK.  
   - Exit adding products anytime by typing `"Done"`.  

4. **Persistent Users**  
   - //All registered users are saved to `users.txt`.  
   - Users are loaded automatically when the program starts.
