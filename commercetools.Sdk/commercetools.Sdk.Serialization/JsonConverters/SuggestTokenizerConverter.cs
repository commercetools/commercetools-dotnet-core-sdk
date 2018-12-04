using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class SuggestTokenizerConverter : JsonConverterDecoratorTypeRetrieverBase<SuggestTokenizer>
    {
        public override string PropertyName => "type";

        public SuggestTokenizerConverter(IDecoratorTypeRetriever<SuggestTokenizer> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}