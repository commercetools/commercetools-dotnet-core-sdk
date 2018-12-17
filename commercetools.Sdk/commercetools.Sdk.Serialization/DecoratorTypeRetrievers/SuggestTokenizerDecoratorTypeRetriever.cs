using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Serialization
{
    public class SuggestTokenizerDecoratorTypeRetriever : DecoratorTypeRetriever<SuggestTokenizer>
    {
        public SuggestTokenizerDecoratorTypeRetriever(IRegisteredTypeRetriever registeredTypeRetriever) : base(registeredTypeRetriever)
        {
        }
    }
}