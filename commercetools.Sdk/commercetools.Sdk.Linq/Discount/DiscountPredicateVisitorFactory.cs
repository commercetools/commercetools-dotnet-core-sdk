using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq.Discount
{
    public class DiscountPredicateVisitorFactory : PredicateVisitorFactoryBase
    {
        public DiscountPredicateVisitorFactory(IEnumerable<IDiscountPredicateVisitorConverter> registeredConverters)
            : base(registeredConverters)
        {
        }
    }
}