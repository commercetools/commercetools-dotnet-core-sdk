using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.ShippingMethods;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class ShippingRatePriceTierConverter : JsonConverterDecoratorTypeRetrieverBase<ShippingRatePriceTier>
    {
        public override string PropertyName => "type";

        public ShippingRatePriceTierConverter(IDecoratorTypeRetriever<ShippingRatePriceTier> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}