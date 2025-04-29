using Library.eCommerce.Models;
using Spring2025_Samples.Models;

namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private InventoryServiceProxy _invSvc = InventoryServiceProxy.Current; //manages a singleton instance of inventory class
        private List<Item> items;   //list of items in cart
        private static ShoppingCartService? instance;   //manages singleton instance of cart class
        public List<Item> CartItems //property to access items in cart
        {
            get
            {
                return items;
            }
        }
        public static ShoppingCartService Current {  //property to access singleton instance of shopping cart
            get
            {
                if(instance == null)
                {
                    instance = new ShoppingCartService();
                }

                return instance;
            } 
        }
        private ShoppingCartService() { //private constructor to prevent external instantiation
            items = new List<Item>();
            //InitializeCart();
        }
        public void InitializeCartIfNeeded() {
            if (!items.Any()) {
                InitializeCart();
            }
        }
        private void InitializeCart()
        {
            // Get some items from inventory
            var item1 = _invSvc.GetById(1); // Product 1
            var item2 = _invSvc.GetById(2); // Product 2
            var item3 = _invSvc.GetById(3); // Product 3

            // Add items to cart
            if (item1 != null)
            {
                AddOrUpdate(item1); // Add 1 unit of Product 1
                AddOrUpdate(item1); // Add another unit
            }

            if (item2 != null)
            {
                AddOrUpdate(item2); // Add 1 unit of Product 2
                AddOrUpdate(item2); // Add another unit
                AddOrUpdate(item2); // Add a third unit
            }

            if (item3 != null)
            {
                AddOrUpdate(item3); // Add 1 unit of Product 3
            } 
        }

        public void AddOrUpdate(Item item)
        {
            // First check if there's enough inventory
            var inventoryItem = _invSvc.GetById(item.Id);
            if (inventoryItem == null || inventoryItem.Quantity <= 0)
            {
                throw new InvalidOperationException("Item is out of stock");
            }

            // Decrement inventory
            inventoryItem.Quantity--;
            _invSvc.AddOrUpdate(inventoryItem);

            // Add to cart
            var existingItem = CartItems.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem == null)
            {
                // Create new cart item
                var newItem = new Item(
                    inventoryItem.Name ?? item.Name ?? item.Product.Name, // Use all possible name sources
                    item.Product,
                    1
                );
                CartItems.Add(newItem);
            }
            else
            {
                // Update existing cart item
                existingItem.Quantity++;
                existingItem.Name = existingItem.Name ?? inventoryItem.Name ?? item.Name ?? item.Product.Name;
            }
        }

        public Item? ReturnItem(Item? item)
        {
            if (item?.Id <= 0 || item == null)
            {
                return null;
            }

            var itemToReturn = CartItems.FirstOrDefault(c => c.Id == item.Id);
            if (itemToReturn != null && itemToReturn.Quantity > 0)
            {
                itemToReturn.Quantity--;
                if (itemToReturn.Quantity == 0)
                {
                    CartItems.Remove(itemToReturn);
                    itemToReturn = null;
                }
                var inventoryItem = _invSvc.GetById(itemToReturn.Id);
                if(inventoryItem != null)
                {
                    inventoryItem.Quantity++;
                    // Ensure name is preserved
                    inventoryItem.Name = itemToReturn.Name ?? inventoryItem.Name;
                    _invSvc.AddOrUpdate(inventoryItem);
                }
                else
                {
                    // Create new inventory item with proper name
                    var newItem = new Item(
                        itemToReturn.Name ?? itemToReturn.Product.Name,
                        itemToReturn.Product,
                        1
                    );
                    _invSvc.AddOrUpdate(newItem);
                }
            }
    
            return itemToReturn;
        }
        
        public Item? ReturnAllItems(Item? item)
        {
            if (item?.Id <= 0 || item == null)
            {
                return null;
            }

            var itemToReturn = CartItems.FirstOrDefault(c => c.Id == item.Id);
            if (itemToReturn != null && itemToReturn.Quantity > 0)
            {
                var inventoryItem = _invSvc.GetById(itemToReturn.Id);
                if(inventoryItem != null)
                {
                    // Add all items back to inventory at once
                    inventoryItem.Quantity += itemToReturn.Quantity;
                    _invSvc.AddOrUpdate(inventoryItem);
                }
                else
                {
                    // Create new inventory item with all quantities if it doesn't exist
                    _invSvc.AddOrUpdate(new Item(itemToReturn.Product.Name, itemToReturn.Product, itemToReturn.Quantity));
                }
        
                // Set cart item quantity to 0
                itemToReturn.Quantity = 0;
            }
    
            return itemToReturn;
        }

        public void Checkout()
        {
            if (!CartItems.Any())
            {
                Console.WriteLine("Cart is empty. Nothing to checkout.");
                return;
            }

            Console.WriteLine("\n========= RECEIPT =========");
            Console.WriteLine("Amazon Store");
            Console.WriteLine("----------------------");
    
            decimal? subtotal = 0;
            foreach (var item in CartItems)
            {
                decimal? itemTotal = (item.Product.Price ?? 0m) * item.Quantity;
                subtotal += itemTotal;
        
                Console.WriteLine($"{item.Name,-20} {item.Quantity,3} @ ${item.Product.Price,6:F2} = ${itemTotal,8:F2}");
            }

            decimal taxRate = 0.07m;
            decimal? tax = subtotal * taxRate;
            decimal? total = subtotal + tax;

            Console.WriteLine("----------------------");
            Console.WriteLine($"{"Subtotal:",-20} ${subtotal,8:F2}");
            Console.WriteLine($"{"Sales Tax (7%):",-20} ${tax,8:F2}");
            Console.WriteLine($"{"Total:",-20} ${total,8:F2}");
            Console.WriteLine("=========================");
            Console.WriteLine("Thank you for shopping!");
            Console.WriteLine($"Date: {DateTime.Now}");
            Console.WriteLine("=========================\n");

            // Clear the cart after checkout
            CartItems.Clear();
        }

    }
}