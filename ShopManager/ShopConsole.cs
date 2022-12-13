using System;
using ShopManager.Products;
using ShopManager.Products.Artwork;

namespace ShopManager
{
    public static class ShopConsole
    {
        public static void PrintSeparator()
        {
            Console.WriteLine("========================");
        }
        
        public static int ShowMenu(Shop shop)
        {
            PrintSeparator();
            Console.WriteLine($"Your actual balance: {shop.Balance} Δρ.");
            Console.WriteLine("1) Sell");
            Console.WriteLine("2) Buy");
            Console.WriteLine("3) Show inventory");
            Console.WriteLine("4) Product's stock");
            Console.WriteLine("5) Use shopping list");
            Console.WriteLine("6) Exit");
            Console.WriteLine("Your choice: ");
            char c = Console.ReadLine()![0];
            if (c is > '0' and < '7')
            {
                return c - '0';
            }
            else
            {
                Console.WriteLine();
                Console.Error.WriteLine("Invalid input");
                return ShowMenu(shop);
            }
        }
        
        public static void PressToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
        
        public static void ShowInventory(Shop shop)
        {
            PrintSeparator();
            if (shop.Products.Length == 0)
            {
                Console.WriteLine("Inventory is empty");
                PressToContinue();
                return;
            }
            Console.WriteLine("Inventory:");
            foreach (var pd in shop.Products)
            {
                if (pd is Stackable st)
                {
                    if(st.Quantity == 1) Console.WriteLine($"\t- {pd.Name} (1 item in stock)");
                    else Console.WriteLine($"\t- {pd.Name} ({st.Quantity} items in stock)");
                }
                else if (pd is Artwork ar)
                {
                    if(ar.InStock) Console.WriteLine($"\t- {pd.Name} (1 item in stock)");
                    else Console.WriteLine($"\t- {pd.Name} (0 item in stock)");
                }
            }
            PressToContinue();
        }
        
        public static void ShowInfo(Shop shop)
        {
            Console.Write("Enter product name: ");
            string s = Console.ReadLine();
            PrintSeparator();
            foreach (var pd in shop.Products)
            {
                if (pd.Name == s)
                {
                    Console.WriteLine($"Name: {pd.Name}");
                    Console.WriteLine($"Price: {pd.Price}");
                    if(pd is Stackable st) Console.WriteLine($"Quantity: {st.Quantity}");
                    else if (pd is Artwork ar)
                    {
                        if(ar.InStock) Console.WriteLine($"Quantity: 1");
                        else Console.WriteLine($"Quantity: 0");
                    }
                    PressToContinue();
                    return;
                }
            }
            Console.WriteLine($"Product '{s}' not found");
            PressToContinue();
        }
        
        public static int ShowProducts((string Name, int Quantity)[] products, string action)
        {
            PrintSeparator();
            if (action == "buy")
            {
                Console.WriteLine("What do you want to buy:");
            }
            else
            {
                Console.WriteLine("What do you want to sell:");
            }
            Console.WriteLine("c) Cancel action");
            for (int i = 0; i < products.Length; i++)
            {
                Console.WriteLine($"{i}) {products[i].Name}");
            }

            char c;
            bool asked = false;
            do
            {
                if(asked) Console.Error.WriteLine("Invalid input");
                asked = true;
                Console.Write("Your choice: ");
                c = Console.ReadLine()![0];
                Console.WriteLine();
            } while (c != 'c' && c < '0' && c - '0' >= products.Length);

            if (c == 'c') return -1;
            return c - '0';
        }
        
        public static void Buy(Shop shop)
        {
            int i = ShowProducts(shop.GetStock(),"buy");
            if(i == -1) return;
            Console.WriteLine("How many do you want to buy?");
            Console.Write("Your choice: ");
            string s = Console.ReadLine();
            Console.WriteLine();
            int n;
            while (!Int32.TryParse(s,out n))
            {
                Console.Error.WriteLine("Invalid input");
                s = Console.ReadLine();
                Console.WriteLine();
            }

            if (n < 0)
            {
                Console.Error.WriteLine("The quantity must be a positive number");
                PressToContinue();
                return;
            }

            var pd = shop.Products[i];
            try
            {
                var bl = shop.Balance;
                shop.Buy(i,n);
                Console.WriteLine($"Bought {n} {pd.Name} for {bl-shop.Balance}Δρ");
            }catch (NotEnoughItemsException e)
            {
                if (pd is Artwork)
                {
                    Console.Error.WriteLine($"Merchant already has {pd.Name} in stock");
                }
            }
            catch (NotEnoughMoneyException e)
            {
                Console.Error.WriteLine($"Not enough money to buy requested amount of {pd.Name}");
            }
            catch (InvalidQuantityException e)
            {
                Console.Error.WriteLine("The quantity must be a positive number");
            }
            catch (IndexOutOfRangeException e)
            {
                Console.Error.WriteLine($"Invalid product index");
            }
            Console.WriteLine();
            PressToContinue();


        }
        
        public static void Sell(Shop shop)
        {
            int i = ShowProducts(shop.GetStock(),"sell");
            if(i == -1)
            {
                Console.WriteLine();
                return;
            }
            Console.WriteLine("How many do you want to sell?");
            Console.Write("Your choice: ");
            string s = Console.ReadLine();
            Console.WriteLine();
            int n;
            while (!Int32.TryParse(s,out n))
            {
                Console.Error.WriteLine("Invalid input");
                Console.Write("Your choice: ");
                s = Console.ReadLine();
                Console.WriteLine();
            }

            if (n < 0)
            {
                Console.Error.WriteLine("The quantity must be a positive number");
                PressToContinue();
                return;
            }

            var pd = shop.Products[i];
            try
            {
                var bl = shop.Balance;
                shop.Sell(i,n);
                Console.WriteLine($"Sold {n} {pd.Name} for {shop.Balance-bl}Δρ");
            }catch (NotEnoughItemsException e)
            {
                if (pd is Artwork)
                {
                    Console.Error.WriteLine($"Merchant already has {pd.Name} in stock");
                }
                else
                {
                    Console.Error.WriteLine($"You can't sell more {pd.Name} than you have");
                }
            }
            catch (InvalidQuantityException e)
            {
                Console.Error.WriteLine($"You can't sell more {pd.Name} than you have");
            }
            catch (IndexOutOfRangeException e)
            {
                Console.Error.WriteLine($"Invalid product index");
            }
            Console.WriteLine();
            PressToContinue();        
        }
        
        public static void HandleShoppingList(Shop shop)
        {
            Console.Write("Your Budget:");
            string s = Console.ReadLine();
            int n = 0;
            while (!Int32.TryParse(s,out n))
            {
                Console.WriteLine("Invalid input");
                Console.Write("Your Budget:");
                s = Console.ReadLine();
            }
            if(n<0) return;
            var sl = new ShoppingList(n);

            string str;
            do
            {
                str = Console.ReadLine();
                if (str == "") break;
                var lss = str.Split(' ');
                int n2 = 0;
                if (lss.Length != 2 || !Int32.TryParse(lss[1],out n2)) 
                { 
                    Console.Error.WriteLine("Shopping list is invalid. Aborting..."); 
                    PressToContinue(); 
                    return;
                }

                var si = new ShoppingItem(lss[0], n2);
                sl.AddItem(si);

            } while (str != "");

            var remain = shop.UseShoppingList(sl);
            Console.WriteLine($"Remaining money: {remain}");
            PressToContinue();

        }
        
        public static void Run(Shop shop)
        {
            int action = 6;
            do
            {
                action = ShowMenu(shop);
                switch (action)
                {
                    case 1:
                        Sell(shop);
                        break;
                    case 2:
                        Buy(shop);
                        break;
                    case 3:
                        ShowInventory(shop);
                        break;
                    case 4:
                        ShowInfo(shop);
                        break;
                    case 5:
                        HandleShoppingList(shop);
                        break;
                    case 6:
                        return;
                    default:
                        break;
                }
            } while (true);


        }
        
    }
}