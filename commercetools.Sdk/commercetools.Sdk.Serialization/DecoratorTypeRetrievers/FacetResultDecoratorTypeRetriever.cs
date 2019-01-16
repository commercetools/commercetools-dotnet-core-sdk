using commercetools.Sdk.Domain;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Serialization
{
    internal class FacetResultDecoratorTypeRetriever : DecoratorTypeRetriever<FacetResult>
    {
        public FacetResultDecoratorTypeRetriever(ITypeRetriever typeRetriever) : base(typeRetriever)
        {
        }
    }
}