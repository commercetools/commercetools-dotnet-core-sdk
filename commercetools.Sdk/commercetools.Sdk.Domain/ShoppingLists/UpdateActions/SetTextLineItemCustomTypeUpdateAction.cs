using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Types;

namespace commercetools.Sdk.Domain.ShoppingLists.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetTextLineItemCustomTypeUpdateAction : UpdateAction<ShoppingList>
    {
        public string Action => "setTextLineItemCustomType";
        [Required]
        public string TextLineItemId { get; set; }
        public IReference<Type> Type { get; set; }
        public Fields Fields { get; set; }
    }
}
