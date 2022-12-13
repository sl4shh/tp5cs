namespace ShopManager.Products.Wine
{
    public class GodBlood : Wine
    {
        protected override float PriceFactor => 4;

        public GodBlood(int age = 100, int quantity = 1)
        {
            this.Age = age;
            this.Quantity = quantity;
        }
    }
}