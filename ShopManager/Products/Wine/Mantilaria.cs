namespace ShopManager.Products.Wine
{
    public class Mantilaria : Wine
    {
        protected override float PriceFactor => 2.3f;

        public Mantilaria(int age = 100, int quantity = 1)
        {
            this.Age = age;
            this.Quantity = quantity;
        }
    }
}