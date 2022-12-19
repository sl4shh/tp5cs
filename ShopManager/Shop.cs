using System;
using ShopManager.Products;
using ShopManager.Products.Artwork;
using ShopManager.Products.Wine;

namespace ShopManager
{
    public class Shop
    {
        public float Balance { get; private set; }
        public Product[] Products { get; }

        public Shop(float balance, Product[] products)
        {
            this.Balance = balance;
            this.Products = products;
        }


        private void AddMoney(float money) => Balance+=money;

        private void RemoveMoney(float money) => Balance -= money;

        public (string Name, int Quantity)[] GetStock()
        {
            var a = new (string Name, int Quantity)[Products.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (Products[i] is Stackable)
                {
                    var st = (Stackable)Products[i];
                    a[i] = (Products[i].Name, st.Quantity);
                }
                else if(Products[i] is Wine w)a[i] = (Products[i].Name +" "+ w.Age+"yrs", 1);
            }

            return a;
        }

        public int FindProductByName(string name)
        {
            int i = 0;
            foreach (var p in Products)
            {
                if (p.Name == name) return i;
                i++;
            }

            return -1;
        }

        public void Buy(int index, int quantity)
        {
            var m = Products.Length;
            if (index < 0 || index >= m) throw new IndexOutOfRangeException();
            if (quantity < 1 || quantity > 1 && Products[index] is not Stackable) throw new InvalidQuantityException();
            var need = Products[index].Price*quantity;
            if (need > this.Balance) throw new NotEnoughMoneyException();
            var p = Products[index];
            if (p is Artwork art)
            {
                if (art.InStock) throw new NotEnoughItemsException();
                art.InStock = true;
                RemoveMoney(need);
            } else if(p is Stackable st)
            {
                st += quantity;
                Products[index] = st;
                RemoveMoney(need);
            }
        }
        
        public void Buy(string name, int quantity)
        {
            var i = FindProductByName(name);
            if (i == -1) throw new IndexOutOfRangeException();
            var p = Products[i];
            if (quantity < 1 || quantity > 1 && p is not Stackable) throw new InvalidQuantityException();
            var need = p.Price * quantity;
            if (need > this.Balance) throw new NotEnoughMoneyException();
            if (p is Artwork art)
            {
                if (art.InStock) throw new NotEnoughItemsException();
                art.InStock = true;
                RemoveMoney(need);
            } else if(p is Stackable st)
            {
                st += quantity;
                Products[i] = st;
                RemoveMoney(need);
            }
        }

        public void Sell(int index, int quantity)
        {
            var m = Products.Length;
            if (index < 0 || index >= m) throw new IndexOutOfRangeException();
            if (quantity < 1 || quantity > 1 && Products[index] is not Stackable) throw new InvalidQuantityException();
            var willGain = Products[index].Price*quantity*Products[index].SaleFactor;
            var p = Products[index];
            if (p is Artwork art)
            {
                if (!art.InStock) throw new NotEnoughItemsException();
                art.InStock = false;
                AddMoney(willGain);
            } else if(p is Stackable st)
            {
                if (st.Quantity < quantity) throw new NotEnoughItemsException();
                st -= quantity;
                Products[index] = st;
                AddMoney(willGain);
            }
        }

        public void Sell(string name, int quantity)
        {
            var i = FindProductByName(name);
            if (i == -1) throw new IndexOutOfRangeException();
            var p = Products[i];
            if (quantity < 1 || quantity > 1 && p is not Stackable) throw new InvalidQuantityException();
            var willGain = Products[i].Price * Products[i].SaleFactor * quantity;
            if (p is Artwork art)
            {
                if (!art.InStock) throw new NotEnoughItemsException();
                art.InStock = false;
                AddMoney(willGain);
            } else if(p is Stackable st)
            {
                if (st.Quantity < quantity) throw new NotEnoughItemsException();
                st -= quantity;
                Products[i] = st;
                AddMoney(willGain);
            }
        }

        public void ShowInfo(string name)
        {
            var i = FindProductByName(name);
            if(i==-1) Console.Error.WriteLine($"Product '{name}' not found");
            else Console.WriteLine(Products[i].GetInfo());
        }
        
        public float UseShoppingList(ShoppingList shoppingList)
        {
            foreach (var si in shoppingList.Items)
            {
                try
                {
                    var firstMoney = this.Balance;
                    Sell(si.Name, si.Quantity);
                    Console.WriteLine($"Bought {si.Quantity} {si.Name}");
                    shoppingList.Budget -= this.Balance - firstMoney;
                }
                catch (NotEnoughItemsException e)
                {
                    Console.Error.WriteLine($"Not enough items to buy {si.Name}");
                }
                catch (NotEnoughMoneyException e)
                {
                    Console.Error.WriteLine($"Not enough money to buy {si.Name}");
                }
                catch (InvalidQuantityException e)
                {
                    Console.Error.WriteLine($"Invalid quantity to buy {si.Name}");
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.Error.WriteLine($"Product '{si.Name}' not found");
                }
            }

            return shoppingList.Budget;
        }
    }
}