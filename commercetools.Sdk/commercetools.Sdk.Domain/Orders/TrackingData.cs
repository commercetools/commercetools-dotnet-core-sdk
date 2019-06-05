namespace commercetools.Sdk.Domain.Orders
{
    public class TrackingData
    {
        public string TrackingId { get; set; }
        public string Carrier { get; set; }
        public string Provider { get; set; }
        public string ProviderTransaction { get; set; }
        public bool IsReturn { get; set; }

        public TrackingData()
        {

        }

        public TrackingData(string trackingId, string carrier, string provider, string providerTransaction,
            bool isReturn)
        {
            this.TrackingId = trackingId;
            this.Carrier = carrier;
            this.Provider = provider;
            this.ProviderTransaction = providerTransaction;
            this.IsReturn = isReturn;
        }
    }
}
