namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddLineItemBySkuUpdateAction : AddLineItemUpdateAction
    {
        public string Sku { get; set; }
    }
}