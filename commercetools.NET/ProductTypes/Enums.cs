using System;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// AttributeConstraint enum tells how an attribute or a set of attributes should be validated across all variants of a product.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#attributeconstraint-enum"/>
    public enum AttributeConstraint
    {
        None,
        Unique,
        CombinationUnique,
        SameForAll
    }

    /// <summary>
    /// TextInputHint enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#textinputhint"/>
    public enum TextInputHint
    {
        SingleLine,
        MultiLine
    }
}
