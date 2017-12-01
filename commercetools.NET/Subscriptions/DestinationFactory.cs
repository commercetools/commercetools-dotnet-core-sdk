namespace commercetools.Subscriptions
{
    public class DestinationFactory
    {
        /// <summary>
        /// Creates a Message using JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        /// <returns>A object derived from Destination, or null</returns>
        public static Destination Create(dynamic data = null)
        {
            if (data == null || data.type == null)
            {
                return null;
            }

            switch ((string)data.type)
            {
                case "IronMQ":
                    return new IronMQDestination(data);
                case "SQS":
                    return new AWSSQSDestination(data);
                case "SNS":
                    return new AWSSNSDestination(data);
            }

            return null;
        }
    }
}
