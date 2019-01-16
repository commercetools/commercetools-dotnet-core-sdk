using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class FacetResultConverter : JsonConverterDecoratorTypeRetrieverBase<FacetResult>
    {
        public override string PropertyName => "type";

        public FacetResultConverter(IDecoratorTypeRetriever<FacetResult> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}