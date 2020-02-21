namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public abstract class OrderUpdateAction: UpdateAction<Order>, IStagedOrderUpdateAction
    {
        public abstract string Action { get; }
    }
}