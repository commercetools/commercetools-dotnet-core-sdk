using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.ProductDiscounts;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    internal class ProductDiscountValueConverter : JsonConverterDecoratorTypeRetrieverBase<ProductDiscountValue>
    {
        public override string PropertyName => "type";

        public ProductDiscountValueConverter(IDecoratorTypeRetriever<ProductDiscountValue> decoratorTypeRetriever) : base(decoratorTypeRetriever)
        {
        }
    }
}