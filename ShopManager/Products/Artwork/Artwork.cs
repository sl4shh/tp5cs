namespace ShopManager.Products.Artwork
{
    public abstract class Artwork : Product
    {
        public override float SaleFactor => 10f;
        public abstract string Artist { get; }
        public abstract int Year { get; }

        public bool InStock { get;  set; }
        

        public override string GetInfo()
        {
            return $"Name: {Name}\nPrice: {Price}\nArtist: {Artist}\nYear: {Year}\nIn Stock: {InStock}\n";
        }

        public override string ToString() => Name;
    }
}