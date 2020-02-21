namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public abstract class CartUpdateAction: UpdateAction<Cart>, IStagedOrderUpdateAction
    {
        public abstract string Action { get; }        
    }
}