using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class CartDiscountValueDecoratorTypeRetriever : DecoratorTypeRetriever<CartDiscountValue>
    {
        public CartDiscountValueDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}