using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class CartDiscountTargetConverter : JsonConverterDecoratorTypeRetrieverBase<CartDiscountTarget>
    {
        public override string PropertyName => "type";

        public CartDiscountTargetConverter(IDecoratorTypeRetriever<CartDiscountTarget> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}