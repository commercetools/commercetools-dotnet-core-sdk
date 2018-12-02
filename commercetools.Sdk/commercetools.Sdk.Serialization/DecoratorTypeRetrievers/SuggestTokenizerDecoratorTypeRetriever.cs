using commercetools.Sdk.Domain;
using commercetools.Sdk.Util;

namespace commercetools.Sdk.Serialization
{
    public class SuggestTokenizerDecoratorTypeRetriever : DecoratorTypeRetriever<SuggestTokenizer>
    {
        public SuggestTokenizerDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}