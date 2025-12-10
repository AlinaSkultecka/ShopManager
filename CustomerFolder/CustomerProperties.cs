using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShopManager.CustomerFolder
{
    public enum CustomerLevel
    {
        BronseLevel,
        SilverLevel,
        GoldLevel
    }
    public class CustomerProperties
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }   // maps to MongoDB _id
        public string Username { get; private set; }
        public string Password { get; private set; }
        public CustomerLevel Level { get; private set; }

        private List<ShoppingCartItem> _shoppingCart;
        public List<ShoppingCartItem> ShoppingCart
        {
            get { return _shoppingCart; }
            set { _shoppingCart = value; }
        }

        // Parameterless constructor for MongoDB
        public CustomerProperties()
        {
            _shoppingCart = new List<ShoppingCartItem>();
        }

        public CustomerProperties(string username, string password, CustomerLevel level = CustomerLevel.BronseLevel)
        {
            Username = username;
            Password = password;
            Level = level;
            _shoppingCart = new List<ShoppingCartItem>();
        }

        public override string ToString()
        {
            return $"{Username},{Password},{Level}";
        }


        //Method to allow users to create a new profile
        public CustomerProperties RegisterUser(string username, string password)
        {
            return new CustomerProperties(username, password);
        }

        //Method to allow users to log in
        public bool Login(string username, string password, List<CustomerProperties> userList)
        {
            foreach (CustomerProperties user in userList)
            {
                if (user.Username == username && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }

        //Method to set user level
        public void SetLevel(CustomerLevel newLevel)
        {
            Level = newLevel;
        }

    }
}
