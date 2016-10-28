using System;

namespace commercetools.Payments
{
    /// <summary>
    /// TransactionState
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#transaction-state"/>
    public enum TransactionState
    {
        Pending,
        Success,
        Failure
    }

    /// <summary>
    /// TransactionType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-payments.html#transaction-type"/>
    public enum TransactionType
    {
        Authorization,
        CancelAuthorization,
        Charge,
        Refund,
        Chargeback
    }
}
