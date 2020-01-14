using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Errors;
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