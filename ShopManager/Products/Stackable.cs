using System;

namespace ShopManager.Products
{
    public abstract class Stackable : Product
    {
        public int Quantity { get; protected set; }

        public static Stackable operator +(Stackable stack, int quantity)
        {
            var s = (Stackable)stack.MemberwiseClone();
            s.Quantity += quantity;
            return s;
        }

        public static Stackable operator -(Stackable stack, int quantity)
        {
            var s = (Stackable)stack.MemberwiseClone();
            s.Quantity -= quantity;
            return s;
        }
    }
}