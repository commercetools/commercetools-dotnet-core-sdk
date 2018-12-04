using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class CartDiscountValueConverter : JsonConverterDecoratorTypeRetrieverBase<CartDiscountValue>
    {
        public override string PropertyName => "type";

        public CartDiscountValueConverter(IDecoratorTypeRetriever<CartDiscountValue> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}