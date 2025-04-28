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

        public string? Display { 
            get
            {
                return $"{Product?.Display ?? string.Empty} Quantity: {Quantity}";
            }
        }

        public Item()   //item default constructor
        {
            Product = new Product();
            Quantity = 0;
            //Id = 0;
            //AddCommand = new Command(DoAdd); for the UI
        }
        private void DoAdd()
        {
            ShoppingCartService.Current.AddOrUpdate(this);
        }

        public Item(String name, Product product, int? quantity)
        {
            Name = name;
            Id = product.Id;
            Product = product;
            Quantity = quantity;
        }
        public Item(Item i)
        {
            Product = new Product(i.Product);
            Quantity = i.Quantity;
            Id = i.Id;
        }


    }
}