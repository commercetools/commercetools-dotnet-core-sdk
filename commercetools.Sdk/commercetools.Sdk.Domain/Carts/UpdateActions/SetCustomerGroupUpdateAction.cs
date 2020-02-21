namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetCustomerGroupUpdateAction : CartUpdateAction
    {
        public override string Action => "setCustomerGroup";
        public ResourceIdentifier CustomerGroup { get; set; }
    }
}