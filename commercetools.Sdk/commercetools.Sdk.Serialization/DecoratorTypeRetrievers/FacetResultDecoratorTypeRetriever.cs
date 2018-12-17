using commercetools.Sdk.Domain;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    public class FacetResultDecoratorTypeRetriever : DecoratorTypeRetriever<FacetResult>
    {
        public FacetResultDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}