using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class FieldTypeConverter : JsonConverterDecoratorTypeRetrieverBase<FieldType>
    {
        public override string PropertyName => "name";

        public FieldTypeConverter(IDecoratorTypeRetriever<FieldType> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}