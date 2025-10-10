using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager
{
    public class ShoppingCartItem
    {
        public ProductFolder.ProductProperties Product { get; set; }
        public int Quantity { get; set; }
        public ShoppingCartItem(ProductFolder.ProductProperties product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
        public decimal TotalPrice => Product.Price * Quantity;
    }
}
