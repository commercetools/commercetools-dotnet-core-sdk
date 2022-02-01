namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetItemShippingAddressCustomTypeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setItemShippingAddressCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}