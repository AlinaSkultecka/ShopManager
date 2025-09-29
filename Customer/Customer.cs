using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager.Customer
{
    public enum UserLevel
    {
        BronseLevel,
        SilverLevel,
        GoldLevel
    }
    public class Customer
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public UserLevel Level { get; private set; }
        private List<ShoppingCartItem> _shoppingCart;
        public List<ShoppingCartItem> ShoppingCart
        {
            get { return _shoppingCart; }
            set { _shoppingCart = value; }
        }
        public Customer(string username, string password, UserLevel level = UserLevel.BronseLevel)
        {
            Username = username;
            Password = password;
            Level = level;
            _shoppingCart = new List<ShoppingCartItem>();
        }

        //Method to allow users to create a new profile
        public Customer RegisterUser(string username, string password)
        {
            return new Customer(username, password);
        }

        //Method to allow users to log in
        public bool Login(string username, string password, List<Customer> userList)
        {
            foreach (Customer user in userList)
            {
                if (user.Username == username && user.Password == password)
                {
                    return true;
                }
            }
             return false;
        }

        //Method to set user level
        public void SetLevel(UserLevel newLevel)
        {
            Level = newLevel;
        }

        //Friendly display ToString()
        public override string ToString()
        {
            return $"Username: {Username}, Level: {Level}, Cart Items: {ShoppingCart.Count}";
        }

    }
}
