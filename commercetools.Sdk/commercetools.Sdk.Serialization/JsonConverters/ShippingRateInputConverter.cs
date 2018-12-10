using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Carts;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class ShippingRateInputConverter : JsonConverterDecoratorTypeRetrieverBase<IShippingRateInput>
    {
        public override string PropertyName => "type";

        public ShippingRateInputConverter(IDecoratorTypeRetriever<IShippingRateInput> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}