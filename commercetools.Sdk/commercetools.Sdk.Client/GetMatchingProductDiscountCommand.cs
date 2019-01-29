using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductDiscounts;

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