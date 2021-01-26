using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseCalculator
{
    public class Program
    {
        private static String[] Options = new String[] { "Add expenses", "Show all expenses", "Show Sum by Category", "Remove Expense", "Remove all Expenses", "Exit program" };
        private static ExpenseCalculator _expenseCalculator = new ExpenseCalculator();
        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Console.WriteLine("Welcome");
            bool isRunning = true;

            while (isRunning)
            {
                int selected = ShowMenu("Choose an option", Options);
                {
                    if (selected == 0)
                    {
                        _expenseCalculator.AddExpense();
                    }
                    else if (selected == 1)
                    {
                        _expenseCalculator.ShowAllExpenses();
                    }
                    else if (selected == 2)
                    {
                        _expenseCalculator.ShowSumByCategory();
                    }
                    else if (selected == 3)
                    {
                        _expenseCalculator.RemoveExpense();
                    }
                    else if (selected == 4)
                    {
                        _expenseCalculator.RemoveAllExpenses();
                    }
                    else
                    {
                        isRunning = false;
                    }
                }
            }
        }

        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        public static decimal ReadDecimal(string prompt)
        {
            Console.Write(prompt);
            return decimal.Parse(Console.ReadLine());
        }
        public static void UserBack()
        {
            Console.WriteLine("Press any key to go back!");
            Console.ReadKey();
            Console.Clear();
        }

        public class Category
        {
            public enum Categories
            {
                Food,
                Entertainment,
                Other,
            }

            public static String[] GetCategoryCategoriess()
            {
                return Enum.GetNames(typeof(Categories)).ToArray();
            }

            public static Categories ParseCategory(String category)
            {
                Object something;
                Categories.TryParse(typeof(Categories), category, true, out something);
                return (Categories)something;
            }
        }

        public class Expense
        {
            public String name { get; set; }
            public Decimal price { get; set; }
            public Category.Categories category { get; set; }

            public Expense(String name, Decimal price, Category.Categories category)
            {
                this.name = name;
                this.price = price;
                this.category = category;
            }
        }

        public class ExpenseCalculator
        {
            public List<Expense> expenses;

            public ExpenseCalculator()
            {
                this.expenses = new List<Expense>();
            }

            public void AddExpense()
            {
                Console.WriteLine("Add expense:");
                String name = ReadString("Name: ");

                Decimal price = ReadDecimal("Price: ");
                var selectedCategory = ShowMenu("Select category!", Category.GetCategoryCategoriess());
                var expense = new Expense(name, price, Category.ParseCategory(Category.GetCategoryCategoriess()[selectedCategory].ToString()));
                this.expenses.Add(expense);
            }
            public Decimal SumExpenses(Category.Categories? category = null)
            {
                decimal sum = 0;
                foreach (Expense e in this.expenses)
                {
                    if (category == null)
                    {
                        // Shows all expenses
                        Console.WriteLine($" - {e.name}: {e.price} SEK ({e.category})");
                        sum += e.price;
                    }
                    else if (e.category == category)
                    {
                        // Shows the sum of expenses in a category
                        sum += e.price;
                    }
                    else
                    {
                        System.Console.WriteLine("An error occurred, no category found.");
                    }
                }
                return sum;
            }
            public void ShowAllExpenses()
            {
                System.Console.WriteLine($"All expenses:");
                decimal sum = SumExpenses();
                System.Console.WriteLine($" Sum: {sum} SEK");
                UserBack();
            }

            public void ShowSumByCategory()
            {
                System.Console.WriteLine($"Sum by category:");
                var categorySums = new Dictionary<Category.Categories, Decimal>();

                foreach (Expense e in this.expenses)
                {
                    if (categorySums.ContainsKey(e.category))
                    {
                        categorySums[e.category] += e.price;
                        continue;
                    }

                    categorySums.Add(e.category, e.price);
                }

                foreach (KeyValuePair<Category.Categories, Decimal> kvp in categorySums)
                {
                    Console.WriteLine($" {kvp.Key}: {kvp.Value} SEK");
                }
                UserBack();
            }
            public void RemoveExpense()
            {
                string[] availableExpenses = new string[expenses.Count];
                for (int i = 0; i < expenses.Count; i++)
                {
                    availableExpenses[i] = expenses[i].name;
                }
                int selection = ShowMenu("Select an expense that you would like to remove:", availableExpenses);

                for (int i = 0; i < expenses.Count; i++)
                {
                    if (selection.ToString() == expenses[i].name)
                    {
                        expenses.Remove(this.expenses[i]);
                        return;
                    }
                }
            }
            public void RemoveAllExpenses()
            {
                String input = ReadString("Are you sure you want to remove all expenses? (Yes/No): ");
                if (input.ToLower() == "yes" || input.ToLower() == "y")
                {
                    expenses.Clear();
                }
            }
        }
        // Don't change this method.
        public static int ShowMenu(string prompt, string[] options)
        {
            if (options == null || options.Length == 0)
            {
                throw new ArgumentException("Cannot show a menu for an empty array of options.");
            }

            Console.WriteLine(prompt);

            int selected = 0;

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                // If this is not the first iteration, move the cursor to the first line of the menu.
                if (key != null)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = Console.CursorTop - options.Length;
                }

                // Print all the options, highlighting the selected one.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("- " + option);
                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }
            }

            // Reset the cursor and return the selected option.
            Console.CursorVisible = true;
            return selected;
        }
    }
}


[TestClass]
public class ProgramTests
{
    [TestMethod]
    public void ExampleTest()
    {
        // Code needed here.
    }
}