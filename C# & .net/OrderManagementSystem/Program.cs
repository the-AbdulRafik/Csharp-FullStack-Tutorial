using System;
using System.Collections.Generic;

namespace OrderManagementSystem
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }

        public Product(int id, string name, double price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }
    }

    class Customer
    {
        public string Name { get; set; }

        public Customer(string name)
        {
            Name = name;
        }
    }

    class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }

    class ShoppingCart
    {
        public List<CartItem> Items { get; set; }

        public ShoppingCart()
        {
            Items = new List<CartItem>();
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be greater than zero.");
                return;
            }

            CartItem existingItem = FindItem(product.Id);

            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > product.Stock)
                {
                    Console.WriteLine("You cannot add more than the available stock.");
                    return;
                }

                existingItem.Quantity += quantity;
            }
            else
            {
                if (quantity > product.Stock)
                {
                    Console.WriteLine("You cannot add more than the available stock.");
                    return;
                }

                Items.Add(new CartItem(product, quantity));
            }

            Console.WriteLine($"{quantity} {product.Name}(s) added to the cart.");
        }

        public void RemoveItem(int productId)
        {
            CartItem item = FindItem(productId);

            if (item == null)
            {
                Console.WriteLine("That product is not in the cart.");
                return;
            }

            Items.Remove(item);
            Console.WriteLine("Item removed from the cart.");
        }

        public CartItem FindItem(int productId)
        {
            foreach (CartItem item in Items)
            {
                if (item.Product.Id == productId)
                {
                    return item;
                }
            }

            return null;
        }

        public double GetSubtotal()
        {
            double subtotal = 0;

            foreach (CartItem item in Items)
            {
                subtotal += item.Product.Price * item.Quantity;
            }

            return subtotal;
        }

        public double GetDiscount()
        {
            if (GetSubtotal() >= 2500)
            {
                return GetSubtotal() * 0.10;
            }

            return 0;
        }

        public double GetShipping(string shippingType)
        {
            if (shippingType == "standard")
            {
                return 80;
            }

            if (shippingType == "express")
            {
                return 180;
            }

            return 0;
        }

        public double GetTotal(string shippingType)
        {
            return GetSubtotal() - GetDiscount() + GetShipping(shippingType);
        }

        public void ShowCart()
        {
            if (Items.Count == 0)
            {
                Console.WriteLine("The cart is empty.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Your cart:");

            foreach (CartItem item in Items)
            {
                double lineTotal = item.Product.Price * item.Quantity;
                Console.WriteLine($"{item.Product.Name} x {item.Quantity} = {lineTotal:C}");
            }

            Console.WriteLine($"Subtotal: {GetSubtotal():C}");
            Console.WriteLine($"Discount: {GetDiscount():C}");
        }
    }

    abstract class Order
    {
        protected ShoppingCart cart;

        public Order(ShoppingCart cart)
        {
            this.cart = cart;
        }

        public abstract double CalculateTotal();
    }

    class DeliveryOrder : Order
    {
        private string shippingType;

        public DeliveryOrder(ShoppingCart cart, string shippingType)
            : base(cart)
        {
            this.shippingType = shippingType;
        }

        public override double CalculateTotal()
        {
            return cart.GetTotal(shippingType);
        }
    }

    class PickupOrder : Order
    {
        public PickupOrder(ShoppingCart cart)
            : base(cart)
        {
        }

        public override double CalculateTotal()
        {
            return cart.GetSubtotal() - cart.GetDiscount();
        }
    }

    abstract class Payment
    {
        public abstract bool Pay(double amount);
    }

    class CardPayment : Payment
    {
        public override bool Pay(double amount)
        {
            Console.WriteLine($"Card payment of {amount:C} was successful.");
            return true;
        }
    }

    class CashPayment : Payment
    {
        public override bool Pay(double amount)
        {
            Console.WriteLine($"Cash on delivery selected for {amount:C}.");
            return true;
        }
    }

    class FailedPayment : Payment
    {
        public override bool Pay(double amount)
        {
            Console.WriteLine("Payment failed.");
            return false;
        }
    }

    class ShoppingService
    {
        private Product[] products;

        public ShoppingService()
        {
            products = new Product[]
            {
                new Product(1, "Laptop", 50000, 5),
                new Product(2, "Mouse", 800, 20),
                new Product(3, "Keyboard", 1200, 15),
                new Product(4, "Monitor", 15000, 8),
                new Product(5, "Headphones", 2500, 12)
            };
        }

        public void ShowProducts()
        {
            Console.WriteLine();
            Console.WriteLine("Products:");

            foreach (Product product in products)
            {
                Console.WriteLine($"{product.Id}. {product.Name} - {product.Price:C} - Stock: {product.Stock}");
            }
        }

        public Product FindProductById(int id)
        {
            foreach (Product product in products)
            {
                if (product.Id == id)
                {
                    return product;
                }
            }

            return null;
        }

        public void SearchProduct(string searchName)
        {
            bool found = false;

            foreach (Product product in products)
            {
                if (product.Name.ToLower().Contains(searchName.ToLower()))
                {
                    Console.WriteLine($"{product.Name} - {product.Price:C} - Stock: {product.Stock}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Product not found.");
            }
        }

        public bool Checkout(Customer customer, ShoppingCart cart, string shippingType, Payment payment)
        {
            if (cart.Items.Count == 0)
            {
                Console.WriteLine("The cart is empty.");
                return false;
            }

            foreach (CartItem item in cart.Items)
            {
                if (item.Quantity > item.Product.Stock)
                {
                    Console.WriteLine($"Not enough stock for {item.Product.Name}.");
                    return false;
                }
            }

            Order order;

            if (shippingType == "pickup")
            {
                order = new PickupOrder(cart);
            }
            else
            {
                order = new DeliveryOrder(cart, shippingType);
            }

            double total = order.CalculateTotal();

            Console.WriteLine();
            Console.WriteLine($"Customer: {customer.Name}");
            Console.WriteLine($"Total amount: {total:C}");

            if (!payment.Pay(total))
            {
                Console.WriteLine("The order was not placed.");
                return false;
            }

            foreach (CartItem item in cart.Items)
            {
                item.Product.Stock -= item.Quantity;
            }

            cart.Items.Clear();
            Console.WriteLine("Order placed successfully.");
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ShoppingService service = new ShoppingService();
            ShoppingCart cart = new ShoppingCart();
            Customer customer = new Customer("Alice");
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("=== SIMPLE ONLINE STORE ===");
                Console.WriteLine("1. Show products");
                Console.WriteLine("2. Search product");
                Console.WriteLine("3. Add product to cart");
                Console.WriteLine("4. Remove product from cart");
                Console.WriteLine("5. Show cart");
                Console.WriteLine("6. Checkout");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        service.ShowProducts();
                        break;

                    case "2":
                        Console.Write("Enter a product name: ");
                        string searchName = Console.ReadLine();
                        service.SearchProduct(searchName);
                        break;

                    case "3":
                        AddProductToCart(service, cart);
                        break;

                    case "4":
                        RemoveProductFromCart(cart);
                        break;

                    case "5":
                        cart.ShowCart();
                        break;

                    case "6":
                        Checkout(service, customer, cart);
                        break;

                    case "7":
                        exit = true;
                        Console.WriteLine("Thank you for shopping.");
                        break;

                    default:
                        Console.WriteLine("Please choose a number from 1 to 7.");
                        break;
                }
            }
        }

        static void AddProductToCart(ShoppingService service, ShoppingCart cart)
        {
            Console.Write("Enter product id: ");
            int productId;

            if (!int.TryParse(Console.ReadLine(), out productId))
            {
                Console.WriteLine("Please enter a valid product id.");
                return;
            }

            Product product = service.FindProductById(productId);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            Console.Write("Enter quantity: ");
            int quantity;

            if (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Please enter a valid quantity.");
                return;
            }

            cart.AddItem(product, quantity);
        }

        static void RemoveProductFromCart(ShoppingCart cart)
        {
            Console.Write("Enter product id to remove: ");
            int productId;

            if (!int.TryParse(Console.ReadLine(), out productId))
            {
                Console.WriteLine("Please enter a valid product id.");
                return;
            }

            cart.RemoveItem(productId);
        }

        static void Checkout(ShoppingService service, Customer customer, ShoppingCart cart)
        {
            if (cart.Items.Count == 0)
            {
                Console.WriteLine("The cart is empty.");
                return;
            }

            Console.Write("Choose shipping (standard, express, pickup): ");
            string shippingType = Console.ReadLine().ToLower();

            if (shippingType != "standard" &&
                shippingType != "express" &&
                shippingType != "pickup")
            {
                Console.WriteLine("Invalid shipping type.");
                return;
            }

            Console.WriteLine("1. Card payment");
            Console.WriteLine("2. Cash on delivery");
            Console.WriteLine("3. Failed payment test");
            Console.Write("Choose payment: ");
            string paymentChoice = Console.ReadLine();

            Payment payment;

            switch (paymentChoice)
            {
                case "1":
                    payment = new CardPayment();
                    break;

                case "2":
                    payment = new CashPayment();
                    break;

                case "3":
                    payment = new FailedPayment();
                    break;

                default:
                    Console.WriteLine("Invalid payment choice.");
                    return;
            }

            service.Checkout(customer, cart, shippingType, payment);
        }
    }
}
