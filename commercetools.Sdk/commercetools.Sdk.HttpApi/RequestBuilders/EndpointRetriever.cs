using commercetools.Sdk.Domain;

namespace commercetools.Sdk.HttpApi.RequestBuilders
{
    public class EndpointRetriever : IEndpointRetriever
    {
        public string GetEndpoint<T>()
        {
            return typeof(T).GetEndpointValue();
        }
    }
}
