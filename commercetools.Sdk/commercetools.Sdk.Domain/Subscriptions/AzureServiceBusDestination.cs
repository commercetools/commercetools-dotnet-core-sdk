namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// Amazon Simple Queue Service (SQS) is a pull-queue on AWS and can be used as a subscription destination.
    /// </summary>
    [TypeMarker("AzureServiceBus")]
    public class AzureServiceBusDestination : Destination
    {
        public string ConnectionString { get; set; }

        public AzureServiceBusDestination(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
