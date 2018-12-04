using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class AttributeTypeConverter : JsonConverterDecoratorTypeRetrieverBase<AttributeType>
    {
        public override string PropertyName => "name";

        public AttributeTypeConverter(IDecoratorTypeRetriever<AttributeType> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}