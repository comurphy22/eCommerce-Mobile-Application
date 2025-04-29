//using Library.eCommerce.DTO;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Library.eCommerce.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Product Product { get; set; }
        public int? Quantity { get; set; }

        
        //public ICommand? AddCommand { get; set; } UI

        public override string ToString()
        {
            return $"{Product} Quantity:{Quantity}";
        }

        public string? Display => $"{Name} (Qty: {Quantity})";

        public Item()
        {
            Product = new Product();
            Quantity = 0;
            Name = Product.Name ?? string.Empty; // Initialize name
        }
        private void DoAdd()
        {
            ShoppingCartService.Current.AddOrUpdate(this);
        }

        public Item(String name, Product product, int? quantity)
        {
            Name = name ?? product.Name; // Use product name if name parameter is null
            Id = product.Id;
            Product = new Product(product);
            Quantity = quantity;
        }
        public Item(Item i)
        {
            Product = new Product(i.Product);
            Quantity = i.Quantity;
            Id = i.Id;
            Name = i.Name ?? i.Product.Name; // Use Product name as fallback
        }


    }
}