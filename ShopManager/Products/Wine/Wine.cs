namespace ShopManager.Products.Wine
{
    public abstract class Wine : Stackable
    {
        public override float SaleFactor => 4.5f;
        protected abstract float PriceFactor { get; }
        public int Age { get; protected set; }
        public override float Price => PriceFactor * Age;

        public override string GetInfo()
        {
            return $"Name: {Name}\nPrice: {Price}\nAge: {Age}\nQuantity: {Quantity}\n";
        }

        public override string ToString() => $"{Name} {Age}yrs";
    }
}