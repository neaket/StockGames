namespace StockGames.Resources
{
    public class Resources
    {
        public Resources() 
        {
        }

        private readonly Common _common = new Common();
        public Common Common
        {
            get { return _common; }
        }
    }
}
