namespace ShopManager.Products
{
    public abstract class Product
    {
        public abstract float Price { get; }
        
        public abstract float SaleFactor { get; }

        public string Name => this.GetType().Name;

        public abstract string GetInfo();
    }
}