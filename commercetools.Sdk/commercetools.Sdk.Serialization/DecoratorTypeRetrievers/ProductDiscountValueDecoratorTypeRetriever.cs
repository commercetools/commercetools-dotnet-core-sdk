using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class ProductDiscountValueDecoratorTypeRetriever : DecoratorTypeRetriever<ProductDiscountValue>
    {
        public ProductDiscountValueDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}