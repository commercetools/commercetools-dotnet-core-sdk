using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class GetMatchingProductDiscountCommand : GetMatchingCommand<ProductDiscount>
    {
        public GetMatchingProductDiscountCommand(IGetMatchingParameters<ProductDiscount> parameters)
            : base(parameters)
        {
        }
    }
}