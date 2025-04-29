using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Microsoft.Maui.ApplicationModel;

namespace Library.eCommerce.Services
{
    public class InventoryServiceProxy
    {
        private InventoryServiceProxy()
        {
            Inventory = new List<Item?>
            {
                new Item
                {
                    Id = 1,
                    Name = "Product 1",
                    Product = new Product
                    {
                        Id = 1,
                        Name = "Product 1",
                        Price = 19.99m
                    },
                    Quantity = 100
                },
                new Item
                {
                    Id = 2,
                    Name = "Product 2",
                    Product = new Product
                    {
                        Id = 2,
                        Name = "Product 2",
                        Price = 29.99m
                    },
                    Quantity = 75
                },
                new Item
                {
                    Id = 3,
                    Name = "Product 3",
                    Product = new Product
                    {
                        Id = 3,
                        Name = "Product 3",
                        Price = 39.99m
                    },
                    Quantity = 50
                }
            };
        }

        public event EventHandler InventoryChanged;

        protected virtual void OnInventoryChanged()
        {
            if (MainThread.IsMainThread)
            {
                InventoryChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() => 
                {
                    InventoryChanged?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        private int LastKey
        {
            get { return Inventory.Any() ? Inventory.Max(p => p?.Id ?? 0) : 0; }
        }

        private static InventoryServiceProxy? instance;
        private static object instanceLock = new object();

        public static InventoryServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new InventoryServiceProxy();
                    }
                }
                return instance;
            }
        }

        public List<Item?> Inventory { get; private set; } = new List<Item?>();

        public Item AddOrUpdate(Item item)
        {
            if (item.Quantity < 0)
            {
                Console.WriteLine("Error: Quantity cannot be negative.");
                return null;
            }

            bool isDuplicateName = Inventory.Any(p => p.Name == item.Name && p.Id != item.Id);
            if (isDuplicateName)
            {
                Console.WriteLine("Duplicate product name in the inventory.");
                return null;
            }

            bool success = false;
            if (item.Id == 0)
            {
                int newId = LastKey + 1;
                item.Id = newId;
                item.Product.Id = newId;
                Inventory.Add(item);
                success = true;
            }
            else
            {
                var existingProduct = Inventory.FirstOrDefault(p => p.Id == item.Id);
                if (existingProduct != null)
                {
                    // Create new item with all properties properly copied
                    var updatedItem = new Item
                    {
                        Id = item.Id,
                        Name = item.Name ?? existingProduct.Name, // Preserve name if not null
                        Product = new Product(item.Product),
                        Quantity = item.Quantity
                    };

                    var index = Inventory.IndexOf(existingProduct);
                    Inventory.RemoveAt(index);
                    Inventory.Insert(index, updatedItem);
                    success = true;
                }
                else
                {
                    Console.WriteLine("Product not found.");
                    return null;
                }
            }

            if (success)
            {
                OnInventoryChanged();
            }

            return item;
        }

        public Item? Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }

            Item? item = Inventory.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                Inventory.Remove(item);
                OnInventoryChanged();
            }

            return item;
        }

        public Item? GetById(int id)
        {
            return Inventory.FirstOrDefault(p => p.Id == id);
        }
    }
}