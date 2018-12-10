namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetCustomFieldUpdateAction : UpdateAction<Order>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}