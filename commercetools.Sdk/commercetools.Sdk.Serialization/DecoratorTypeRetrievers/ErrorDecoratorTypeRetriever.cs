using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    public class ErrorDecoratorTypeRetriever : DecoratorTypeRetriever<Error>
    {
        public ErrorDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}