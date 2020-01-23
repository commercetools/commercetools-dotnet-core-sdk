namespace commercetools.Sdk.Domain.OrderEdits
{
    public abstract class StagedOrderUpdateAction : StagedUpdateAction<OrderEdit>
    {
        public abstract string Action { get; }
    }
}