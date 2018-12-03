using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class ProductDiscountValueDecoratorTypeRetriever : DecoratorTypeRetriever<ProductDiscountValue>
    {
        public ProductDiscountValueDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}