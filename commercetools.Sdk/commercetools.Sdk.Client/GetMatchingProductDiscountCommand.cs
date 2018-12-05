namespace commercetools.Sdk.Client
{
    using Domain;

    // TODO Implement a request builder for this command
    public class GetMatchingProductDiscountCommand : Command<ProductDiscount>
    {
        public override System.Type ResourceType => typeof(ProductDiscount);
    }
}