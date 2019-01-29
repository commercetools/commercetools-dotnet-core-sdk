using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.ProductDiscounts;

namespace commercetools.Sdk.Serialization
{
    internal class ProductDiscountValueDecoratorTypeRetriever : DecoratorTypeRetriever<ProductDiscountValue>
    {
        public ProductDiscountValueDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}