namespace ShopManager.Products.Food
{
    public class Moussaka : Food
    {
        public override float Price => 30f;

        public Moussaka(int quantity = 1)
        {
            this.Quantity = quantity;
        }
    }
}