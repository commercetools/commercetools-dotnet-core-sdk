using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization.DecoratorTypeRetrievers
{
    public class ErrorDecoratorTypeRetriever : DecoratorTypeRetriever<Error, ErrorTypeAttribute>
    {
        public ErrorDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}