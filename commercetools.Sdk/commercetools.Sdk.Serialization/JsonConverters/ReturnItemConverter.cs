using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Orders;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class ReturnItemConverter : JsonConverterDecoratorTypeRetrieverBase<ReturnItem>
    {
        public override string PropertyName => "type";

        public ReturnItemConverter(IDecoratorTypeRetriever<ReturnItem> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}