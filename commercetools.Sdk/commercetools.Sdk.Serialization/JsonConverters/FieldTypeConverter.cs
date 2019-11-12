using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Types.FieldTypes;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class FieldTypeConverter : JsonConverterDecoratorTypeRetrieverBase<FieldType>
    {
        public override string PropertyName => "name";

        public FieldTypeConverter(IDecoratorTypeRetriever<FieldType> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}