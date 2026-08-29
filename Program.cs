using System;
using System.Collections.Generic;

// =====================================================================================
//  SHOPMASTER - ONLINE STORE ORDER PROCESSING
//  Single-file console app: open in Visual Studio and press Run (F5 / Ctrl+F5).
//
//  Design goal stated by the manager: "don't hardcode anything... plug in new
//  behavior without changing existing code." That's exactly what delegates give us -
//  SearchProducts/PrintReport/TransformProducts/FilterProducts never change; only the
//  lambda passed in at the call site changes.
//
//  NOTE: Task 02 does not exist in the original assignment document - it jumps
//  directly from Task 01 to Task 03, so this file does the same.
// =====================================================================================

namespace ShopMasterAssignment
{
    // =================================================================================
    // Starter Code: Data Models & Product Catalog
    // =================================================================================
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } // "Electronics", "Clothing", "Food", "Books"
        public double Price { get; set; }
        public int Stock { get; set; }
    }


    internal class Program
    {
        static void Main()
        {
            List<Product> catalog = new()
            {
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200, Stock = 10 },
                new Product { Id = 2, Name = "Phone", Category = "Electronics", Price = 800, Stock = 25 },
                new Product { Id = 3, Name = "T-Shirt", Category = "Clothing", Price = 30, Stock = 100 },
                new Product { Id = 4, Name = "Jeans", Category = "Clothing", Price = 60, Stock = 50 },
                new Product { Id = 5, Name = "Chocolate", Category = "Food", Price = 5, Stock = 200 },
                new Product { Id = 6, Name = "Coffee Beans", Category = "Food", Price = 15, Stock = 80 },
                new Product { Id = 7, Name = "C# Book", Category = "Books", Price = 45, Stock = 30 },
                new Product { Id = 8, Name = "Novel", Category = "Books", Price = 20, Stock = 60 },
                new Product { Id = 9, Name = "Headphones", Category = "Electronics", Price = 150, Stock = 40 },
                new Product { Id = 10, Name = "Jacket", Category = "Clothing", Price = 120, Stock = 15 },
            };

            // =============================================================================
            // TASK 01: Smart Product Search
            // =============================================================================
            // Delegate used: Func<Product, bool>
            // Why: we need a delegate that takes ONE Product and returns a bool (does it
            // match the filter?). Func<T, TResult> is exactly that shape - the caller
            // supplies whatever filter logic it wants as a lambda, and SearchProducts()
            // itself never needs to change to support a new kind of search.

            Console.WriteLine("--- Electronics ---");
            foreach (Product p in SearchProducts(catalog, p => p.Category == "Electronics"))
            {
                Console.WriteLine($"{p.Name} - ${p.Price} (Stock: {p.Stock})");
            }

            Console.WriteLine();
            Console.WriteLine("--- Under $50 ---");
            foreach (Product p in SearchProducts(catalog, p => p.Price < 50))
            {
                Console.WriteLine($"{p.Name} - ${p.Price} (Stock: {p.Stock})");
            }

            Console.WriteLine();
            Console.WriteLine("--- In Stock ---");
            foreach (Product p in SearchProducts(catalog, p => p.Stock > 0))
            {
                Console.WriteLine($"{p.Name} - ${p.Price} (Stock: {p.Stock})");
            }

            Console.WriteLine();
            Console.WriteLine("--- Clothing Under $100 ---");
            foreach (Product p in SearchProducts(catalog, p => p.Category == "Clothing" && p.Price < 100))
            {
                Console.WriteLine($"{p.Name} - ${p.Price} (Stock: {p.Stock})");
            }

            // =============================================================================
            // TASK 03.1: Print Reports
            // =============================================================================
            // Delegate used: Action<Product> (built-in delegate)
            // Why: PrintReport() doesn't need to return anything - it just needs to DO
            // something (print) with each product. Action<T> is the built-in delegate
            // shape for "takes a T, returns nothing", so there's no need to declare a
            // custom delegate type at all.

            Console.WriteLine();
            Console.WriteLine("--- Short Report ---");
            PrintReport(catalog, p => Console.WriteLine($"{p.Name} - ${p.Price}"));

            Console.WriteLine();
            Console.WriteLine("--- Detailed Report ---");
            PrintReport(catalog, p => Console.WriteLine($"[{p.Category}] {p.Name} | Price: ${p.Price} | Stock: {p.Stock}"));

            // =============================================================================
            // TASK 03.2: Transform Products
            // =============================================================================
            // Delegate used: Func<Product, string> (built-in delegate)
            // Why: TransformProducts() takes each Product IN and produces a new string
            // OUT - a genuine transformation/mapping, not just an action. Func<T, TResult>
            // is the built-in delegate for exactly that "take one thing, return another"
            // shape.

            Console.WriteLine();
            Console.WriteLine("--- Summary List ---");
            List<string> summaries = TransformProducts(catalog, p => $"{p.Name} (${p.Price})");
            foreach (string s in summaries)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine();
            Console.WriteLine("--- Price Labels ---");
            List<string> labels = TransformProducts(catalog, p => p.Price > 100 ? "Expensive!" : "Affordable");
            for (int i = 0; i < catalog.Count; i++)
            {
                Console.WriteLine($"{catalog[i].Name}: {labels[i]}");
            }

            // =============================================================================
            // TASK 03.3: Filter Products
            // =============================================================================
            // Delegate used: Predicate<Product> (built-in delegate)
            // Why: Predicate<T> is the built-in delegate made specifically for a
            // true/false test on a single T - semantically the clearest choice for
            // "does this product match a yes/no condition?", even though it behaves the
            // same as Func<Product, bool> under the hood.

            Console.WriteLine();
            Console.WriteLine("--- Low-Stock Alert ---");
            List<Product> lowStock = FilterProducts(catalog, p => p.Stock < 20);
            foreach (Product p in lowStock)
            {
                Console.WriteLine($"[LOW STOCK] {p.Name}: only {p.Stock} left!");
            }

            Console.WriteLine();
            Console.WriteLine("===== Done. Press any key to exit. =====");
            Console.ReadKey();
        }

        // Task 01: ONE search method that works for any filter, now and in the future,
        // without ever needing to be modified.
        static List<Product> SearchProducts(List<Product> products, Func<Product, bool> condition)
        {
            List<Product> result = new List<Product>();
            foreach (Product p in products)
            {
                if (condition(p))
                {
                    result.Add(p);
                }
            }
            return result;
        }

        // Task 3.1: caller decides what to print by passing a lambda (Action<Product>).
        static void PrintReport(List<Product> products, Action<Product> printAction)
        {
            foreach (Product p in products)
            {
                printAction(p);
            }
        }

        // Task 3.2: caller decides how to transform each product into a string.
        static List<string> TransformProducts(List<Product> products, Func<Product, string> transform)
        {
            List<string> results = new List<string>();
            foreach (Product p in products)
            {
                results.Add(transform(p));
            }
            return results;
        }

        // Task 3.3: caller decides which products match, via a Predicate<Product>.
        static List<Product> FilterProducts(List<Product> products, Predicate<Product> condition)
        {
            List<Product> result = new List<Product>();
            foreach (Product p in products)
            {
                if (condition(p))
                {
                    result.Add(p);
                }
            }
            return result;
        }
    }
}
