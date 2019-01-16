using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    internal class ReturnItemDecoratorTypeRetriever : DecoratorTypeRetriever<ReturnItem>
    {
        public ReturnItemDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}