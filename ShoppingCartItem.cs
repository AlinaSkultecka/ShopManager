using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager
{
    public class ShoppingCartItem
    {
        public Product.ProductDetails Product { get; set; }
        public int Quantity { get; set; }
        public ShoppingCartItem(Product.ProductDetails product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
        public decimal TotalPrice => Product.Price * Quantity;
    }
}
