using System;
using System.Linq.Expressions;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Domain.Query
{
    public class AttributeExpansion<T> : Expansion<T, Attribute> where T : Product
    {
        public AttributeExpansion(Expression<Func<T, Attribute>> expression) : base(expression)
        {
        }
    }
}
