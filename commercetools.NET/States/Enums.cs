using System;

namespace commercetools.States
{
    /// <summary>
    /// StateRole enumeration.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-states.html#staterole"/>
    public enum StateRole
    {
        ReviewIncludedInStatistics
    }

    /// <summary>
    /// StateType enumeration.
    /// </summary>
    /// <see href="https://dev.commercetools.com/http-api-projects-states.html#statetype"/>
    public enum StateType
    {
        OrderState,
        LineItemState,
        ProductState,
        ReviewState,
        PaymentState
    }
}
