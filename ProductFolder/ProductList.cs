using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager.ProductFolder
{
    public class SoftToy : ProductFolder.ProductProperties { }
    public class  Lego : ProductFolder.ProductProperties { }
    public class BabyToy : ProductFolder.ProductProperties { }
    public class BoardGame : ProductFolder.ProductProperties { }

    // Get randome id
    public static class IdGenerator
    {
        private static int _currentId = 0;

        public static int NextId() => ++_currentId;

        public static void Initialize(int startValue)
        {
            _currentId = startValue;
        }
    }

    public class ProductList
    {
         public static SoftToy[] SoftToys = new SoftToy[] {
             new SoftToy {Id = IdGenerator.NextId(), Name = "Teddy Bear", ProductCategory = "SoftToy", Price = 199.99m, Stock = 50},
             new SoftToy {Id = IdGenerator.NextId(), Name = "Bunny", ProductCategory = "SoftToy", Price = 149.99m, Stock = 30},
             new SoftToy {Id = IdGenerator.NextId(), Name = "Dog", ProductCategory = "SoftToy", Price = 249.99m, Stock = 20}
             };
         public static Lego[] Lego = new Lego[] {
             new Lego {Id = IdGenerator.NextId(), Name = "Lego City", ProductCategory = "Lego", Price = 499.99m, Stock = 15},
             new Lego {Id = IdGenerator.NextId(), Name = "Lego Star Wars", ProductCategory = "Lego", Price = 599.99m, Stock = 10},
             new Lego {Id = IdGenerator.NextId(), Name = "Lego Friends", ProductCategory = "Lego", Price = 399.99m, Stock = 25}
             };
         public static BabyToy[] BabyToy = new BabyToy[] {
             new BabyToy {Id = IdGenerator.NextId(), Name = "Rattle", ProductCategory = "BabyToy", Price = 99.99m, Stock = 40},
             new BabyToy {Id = IdGenerator.NextId(), Name = "Teething Ring", ProductCategory = "BabyToy", Price = 129.99m, Stock = 35},
             new BabyToy {Id = IdGenerator.NextId(), Name = "Soft Book", ProductCategory = "BabyToy", Price = 149.99m, Stock = 20}
             };
         public static BoardGame[] BoardGame = new BoardGame[] {
             new BoardGame {Id = IdGenerator.NextId(), Name = "Monopoly", ProductCategory = "BoardGame", Price = 299.99m, Stock = 20},
             new BoardGame {Id = IdGenerator.NextId(), Name = "Scrabble", ProductCategory = "BoardGame", Price = 249.99m, Stock = 15},
             new BoardGame {Id = IdGenerator.NextId(), Name = "Catan", ProductCategory = "BoardGame", Price = 349.99m, Stock = 10}
             };

        // Combine all categories into one list
        public static List<ProductProperties> AllProducts =>
            SoftToys.Cast<ProductProperties>()
            .Concat(Lego)
            .Concat(BabyToy)
            .Concat(BoardGame)
            .ToList(); 
    }
}
