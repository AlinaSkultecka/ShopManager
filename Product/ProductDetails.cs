using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager.Product
{
    public class ProductDetails
    {
        public int Id { get; set; } 
        public string ProductCategory { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ProductDetails(int id, string ProductCategory, string name, decimal price, int stock)
        {
            Id = id;
            this.ProductCategory = ProductCategory;
            Name = name;
            Price = price;
            Stock = stock;
        }
        public ProductDetails() { }  //it is useed for initialization in ProductsList.cs


    }
}
