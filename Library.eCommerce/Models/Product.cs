using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring2025_Samples.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //public int Quantity { get; set;  only for cart items?
        
        public decimal? Price { get; set; }

        public string? Display
        {
            get
            {
                return $"{Id}. {Name}";
            }
        }

        public int Quantity { get; set; }

        public Product()
        {
            Name = string.Empty;
        }
        
        public Product(Product p)
        {
            Id = p.Id;
            Name = p.Name;
            Price = p.Price;
            Quantity = p.Quantity;
        }

        public Product(int id, string name, decimal? price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Id}. {Name} - {Price}";
        }
        }
    }
