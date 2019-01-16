using commercetools.Sdk.HttpApi.Domain;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    internal class ErrorDecoratorTypeRetriever : DecoratorTypeRetriever<Error>
    {
        public ErrorDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}