using System;

namespace commercetools.Orders
{
    /// <summary>
    /// OrderState enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#order-state"/>
    public enum OrderState
    {
        Open,
        Confirmed,
        Complete,
        Cancelled
    }

    /// <summary>
    /// PaymentState enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#paymentstate"/>
    public enum PaymentState
    {
        BalanceDue,
        Failed,
        Pending,
        CreditOwed,
        Paid
    }

    /// <summary>
    /// ReturnPaymentState enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#returnpaymentstate"/>
    public enum ReturnPaymentState
    {
        NonRefundable,
        Initial,
        Refunded,
        NotRefunded
    }

    /// <summary>
    /// ReturnShipmentState enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#returnshipmentstate"/>
    public enum ReturnShipmentState
    {
        Advised,
        Returned,
        BackInStock,
        Unusable
    }

    /// <summary>
    /// ShipmentState enumeration.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#shipmentstate"/>
    public enum ShipmentState
    {
        Shipped,
        Ready,
        Pending,
        Delayed,
        Partial,
        Backorder
    }
}
