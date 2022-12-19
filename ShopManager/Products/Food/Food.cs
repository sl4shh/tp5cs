using System;

namespace ShopManager.Products.Food
{
    public abstract class Food : Stackable
    {
        public override float SaleFactor => 1.2f;

        public override string GetInfo()
        {
            return $"Name: {this.Name}\nPrice: {this.Price}\nQuantity: {this.Quantity}\n";
        }

        public override string ToString() => Name;
    }
}