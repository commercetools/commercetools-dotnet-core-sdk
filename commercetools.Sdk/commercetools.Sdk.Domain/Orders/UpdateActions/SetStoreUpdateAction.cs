using commercetools.Sdk.Domain.Stores;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetStoreUpdateAction : OrderUpdateAction
    {
        public override string Action => "setStore";
        public ResourceIdentifier<Store> Store{ get; set; }
    }
}