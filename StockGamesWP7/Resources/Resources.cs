namespace StockGames.Resources
{
    /// <summary>   Resources is used to contain and local specific resources. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class Resources
    {
        private readonly Common _common = new Common();

        /// <summary>   Gets the common resources for this application. </summary>
        ///
        /// <value> The common resources. </value>
        public Common Common
        {
            get { return _common; }
        }
    }
}
