namespace commercetools.Sdk.Client
{
    using Domain;

    public class SearchProductProjectionsCommand : SearchCommand<ProductProjection>
    {
        public override System.Type ResourceType => typeof(ProductProjection);
    }
}