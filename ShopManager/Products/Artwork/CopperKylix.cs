namespace ShopManager.Products.Artwork
{
    public class CopperKylix : Artwork
    {
        public override float Price => 100;
        public override string Artist => "Fecarabos";
        public override int Year => 300;

        public CopperKylix(bool inStock = true)
        {
            this.InStock = inStock;
        }
    }
}