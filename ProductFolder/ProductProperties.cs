using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager.ProductFolder
{
    public class ProductProperties
    {
        // This maps to MongoDB _id (ObjectId)
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }
        public int Id { get; set; } 
        public string ProductCategory { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ProductProperties(int id, string ProductCategory, string name, decimal price, int stock)
        {
            Id = id;
            this.ProductCategory = ProductCategory;
            Name = name;
            Price = price;
            Stock = stock;
        }
        public ProductProperties() { }  //it is useed for initialization in ProductsList.cs


    }
}
