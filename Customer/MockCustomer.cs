using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager.Customer
{
    public class MockCustomer
    {
        public List<Customer> Customers { get; private set; } = new List<Customer>();

        public MockCustomer()
        {
            Customers.Add(new Customer("aa", "11", UserLevel.BronseLevel));
            Customers.Add(new Customer("Knatte", "123", UserLevel.BronseLevel));
            Customers.Add(new Customer("Fnatte", "321", UserLevel.BronseLevel));
            Customers.Add(new Customer("Tjatte", "213", UserLevel.BronseLevel));
        }
    }
}
