using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.OrderEdits;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class OrderEditResultConverter : JsonConverterDecoratorTypeRetrieverBase<OrderEditResult>
    {
        public override string PropertyName => "type";

        public OrderEditResultConverter(IDecoratorTypeRetriever<OrderEditResult> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}