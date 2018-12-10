using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class ReturnItemDecoratorTypeRetriever : DecoratorTypeRetriever<ReturnItem>
    {
        public ReturnItemDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}