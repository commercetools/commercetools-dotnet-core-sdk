namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetShippingAddressCustomTypeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setShippingAddressCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}