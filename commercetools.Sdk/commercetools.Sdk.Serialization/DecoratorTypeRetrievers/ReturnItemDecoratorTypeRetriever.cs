using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    public class ReturnItemDecoratorTypeRetriever : DecoratorTypeRetriever<ReturnItem>
    {
        public ReturnItemDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}