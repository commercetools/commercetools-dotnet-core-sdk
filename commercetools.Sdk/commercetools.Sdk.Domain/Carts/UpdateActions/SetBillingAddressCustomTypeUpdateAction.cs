namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetBillingAddressCustomTypeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setBillingAddressCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}