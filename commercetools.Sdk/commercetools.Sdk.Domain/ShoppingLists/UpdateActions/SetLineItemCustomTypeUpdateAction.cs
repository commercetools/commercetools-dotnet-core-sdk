using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemCustomTypeUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setLineItemCustomType";
        [Required]
        public string LineItemId { get; set; }
        public IReference<Type> Type { get; set; }
        public Fields Fields { get; set; }
    }
}
