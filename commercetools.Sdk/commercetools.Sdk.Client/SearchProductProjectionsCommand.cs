using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class SearchProductProjectionsCommand : SearchCommand<ProductProjection>
    {
        public override System.Type ResourceType => typeof(ProductProjection);
    }
}