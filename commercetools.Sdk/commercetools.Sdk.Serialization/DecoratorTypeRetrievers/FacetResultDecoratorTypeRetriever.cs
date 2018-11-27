using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class FacetResultDecoratorTypeRetriever : DecoratorTypeRetriever<FacetResult, FacetResultTypeAttribute>
    {
        public FacetResultDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}