using System;
using ShopManager.Products;

namespace ShopManager
{
    public class ShoppingItem
    {
        public readonly string Name;
        public readonly int Quantity;

        public ShoppingItem(string name, int quantity)
        {
            this.Name = name;
            this.Quantity = quantity;
        }
    }
    public class ShoppingList
    {
        public float Budget { get; set; }
        public ShoppingItem[] Items { get; private set; }

        public ShoppingList(int budget)
        {
            this.Budget = budget;
            this.Items = new ShoppingItem[] { };
        }

        public ShoppingList(int budget, ShoppingItem[] items)
        {
            this.Budget = budget;
            if (items.Length == 0) this.Items = new ShoppingItem[]{};
            else this.Items = items;
        }


        public void AddItem(ShoppingItem item)
        {
            var cp = new ShoppingItem[Items.Length + 1];

            for (int i = 0; i < Items.Length; i++)
            {
                if (item.Name == Items[i].Name)
                {
                    Items[i] = new ShoppingItem(Items[i].Name, Items[i].Quantity + item.Quantity);
                    return;
                }

                cp[i] = Items[i];
            }
            cp[Items.Length] = item;
            
            Items = cp;
        }
    }
}