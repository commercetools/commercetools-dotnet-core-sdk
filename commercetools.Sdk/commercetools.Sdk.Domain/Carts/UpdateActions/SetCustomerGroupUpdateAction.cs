namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using commercetools.Sdk.Domain.Validation.Attributes;

    public class SetCustomerGroupUpdateAction : UpdateAction<Cart>
    {
        public string Action => "setCustomerGroup";
        public ResourceIdentifier CustomerGroup { get; set; }
    }
}