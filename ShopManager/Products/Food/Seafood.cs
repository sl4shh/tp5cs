namespace ShopManager.Products.Food
{
    public class Seafood : Food
    {
        public override float Price => 10f;

        public Seafood(int quantity = 1)
        {
            Quantity = quantity;
        }
    }
}