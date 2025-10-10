using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShopManager.CustomerFolder
{
    public class CustomerConstructor // is used to create mock customers from the text file
    {
        public List<CustomerProperties> Customers { get; private set; } = new List<CustomerProperties>();
        string filePath = "C:\\Users\\grigo\\Desktop\\C#\\3. Inlämning upgifter\\ShopManager\\CustomerFolder\\MockCustomers.txt";

        public CustomerConstructor()  
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
                        CustomerLevel level;

                        // Convert user level to enum value
                        if (Enum.TryParse(parts[2], out level))
                        {
                            Customers.Add(new CustomerProperties(name, password, level));
                        }
                    }
                }
            }
        }
    }
}
