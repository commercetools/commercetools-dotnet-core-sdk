using commercetools.Sdk.HttpApi.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class ErrorConverter : JsonConverterDecoratorTypeRetrieverBase<Error>
    {
        public override string PropertyName => "code";
        public override Type DefaultType => typeof(GeneralError);

        public ErrorConverter(IDecoratorTypeRetriever<Error> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}