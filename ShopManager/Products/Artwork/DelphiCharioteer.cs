namespace ShopManager.Products.Artwork
{
    public class DelphiCharioteer : Artwork
    {
        public override float Price => 130;
        public override string Artist => "Mixomatos";
        public override int Year => 500;

        public DelphiCharioteer(bool inStock = true)
        {
            this.InStock = inStock;
        }
    }
}