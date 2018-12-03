using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    // TODO Implement a request builder for this command
    public class GetMatchingProductDiscountCommand : Command<ProductDiscount>
    {
        public override System.Type ResourceType => typeof(ProductDiscount);
    }
}