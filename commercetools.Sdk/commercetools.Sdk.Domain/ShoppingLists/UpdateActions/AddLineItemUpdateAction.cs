namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddLineItemUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "addLineItem";
        [Required]
        public string ProductId { get; set; }
        public int? VariantId { get; set; }
        public long Quantity { get; set; }
        public DateTime? AddedAt { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}
