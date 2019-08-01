namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddTextLineItemUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "addTextLineItem";
        [Required]
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public long? Quantity { get; set; }
        public DateTime? AddedAt { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}
