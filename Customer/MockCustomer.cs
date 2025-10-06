using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShopManager.Customer
{
    public class MockCustomer
    {
        public List<Customer> Customers { get; private set; } = new List<Customer>();
        string filePath = "C:\\Users\\grigo\\Desktop\\C#\\3. Inlämning upgifter\\ShopManager\\Customer\\MockCustomers.txt";

        public MockCustomer()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 1; i < lines.Length; i++)  // Start from 1 to skip header
                {
                    string line = lines[i];

                    string[] parts = line.Split(',');

                    if (parts.Length == 3)
                    {
                        string name = parts[0];
                        string password = parts[1];
                        UserLevel level;

                        // Convert user level to enum value
                        if (Enum.TryParse(parts[2], out level))
                        {
                            Customers.Add(new Customer(name, password, level));
                        }
                    }
                }
            }
        }
    }
}
