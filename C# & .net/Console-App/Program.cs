// Build an Retail Management console App

using System;

namespace RetailManagementApp
{
    class Program
    {
        // public static void DisplayWelcome()
        // {
        //     Console.WriteLine("Welcome to the Retail Management Console App !!");
        //     Console.WriteLine("This Console will help you manage your retail orders efficiently.");
        // }
        public static void Main(string[] args)
        {
            int orderId;
            string customerName;
            string productName;
            int quantity;
            decimal unitPrice;
            string orderType;
            decimal orderTotal = 0m;
            decimal grandTotal = 0m;

            while (true)
            {
                Console.WriteLine("Enter Order ID (or type 'exit' to quit):");
                string? input = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Order ID cannot be empty. Please enter a valid Order ID.");
                    continue;
                }
                if (input == null || input.ToLower().Equals("exit"))
                {
                    break;
                }
                orderId = int.Parse(input);
                Console.WriteLine("Enter Customer Name:");
                customerName = Console.ReadLine()!;
                while(string.IsNullOrEmpty(customerName))
                {
                    Console.WriteLine("Customer Name cannot be empty. Please enter a valid name.");
                    Console.WriteLine("Enter Customer Name:");
                    customerName = Console.ReadLine()!;
                }

                // Product 
                Console.WriteLine("Enter Product Name:");
                productName = Console.ReadLine()!;
                while(string.IsNullOrEmpty(productName))
                {
                    Console.WriteLine("Product Name cannot be empty. Please enter a valid name.");
                    Console.WriteLine("Enter Product Name:");
                    productName = Console.ReadLine()!;
                }
                Console.WriteLine("Enter Quantity:");
                bool isValidQuantity = int.TryParse(Console.ReadLine(), out quantity);
                while (!isValidQuantity || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity. Please enter a + integer for Quantity:");
                    isValidQuantity = int.TryParse(Console.ReadLine(), out quantity);
                }

                //Price 
                Console.WriteLine("Enter Unit Price:");
                bool isValidUnitPrice = decimal.TryParse(Console.ReadLine(), out unitPrice);
                while (!isValidUnitPrice || unitPrice <= 0)
                {
                    Console.WriteLine("Invalid unit price. Please enter Correct Decimalfor Unit Price:");
                    isValidUnitPrice = decimal.TryParse(Console.ReadLine(), out unitPrice);
                }

                // Calculate the total price of the order
                orderTotal = quantity * unitPrice;
                // classify the order based on the total price
                if(orderTotal < 1000)
                {
                    orderType = "Small Order";
                }else if(orderTotal >= 1000 && orderTotal < 5000)
                {
                    orderType = "Medium Order";
                }
                else if(orderTotal >= 5000 && orderTotal < 10000)
                {
                    orderType = "Large Order";
                }
                else
                {
                    orderType = "bulk Order";
                }
                Console.WriteLine("--------Order Details-------- ");
                Console.WriteLine($"Order ID: {orderId}");
                Console.WriteLine($"Customer Name: {customerName}");
                Console.WriteLine($"Product Name: {productName}");
                Console.WriteLine($"Quantity: {quantity}");
                Console.WriteLine($"Unit Price: {unitPrice:F2}");
                Console.WriteLine($"Order Type: {orderType}");

                Console.WriteLine($"Order Total: {orderTotal:F2}");
                Console.WriteLine(" Press y to confirm the order or any other key to cancel:");
                string? confirm = Console.ReadLine();
                if (confirm == null || !confirm.ToLower().Equals("y"))
                {
                    Console.WriteLine("Order Cancelled.");
                    continue;
                }
                grandTotal += orderTotal;
            }
            Console.WriteLine($"Grand Total: {grandTotal:F2}");
        }
    }
};