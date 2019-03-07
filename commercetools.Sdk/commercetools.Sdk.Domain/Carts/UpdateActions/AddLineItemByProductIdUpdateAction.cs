namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddLineItemByProductIdUpdateAction : AddLineItemUpdateAction
    {
        public string ProductId { get; set; }
        public int VariantId { get; set; } = 1;//By Default Master Variant
    }
}