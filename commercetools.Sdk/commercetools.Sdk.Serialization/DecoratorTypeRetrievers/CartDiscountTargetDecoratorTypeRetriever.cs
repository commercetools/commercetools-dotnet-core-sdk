using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    internal class CartDiscountTargetDecoratorTypeRetriever : DecoratorTypeRetriever<CartDiscountTarget>
    {
        public CartDiscountTargetDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}