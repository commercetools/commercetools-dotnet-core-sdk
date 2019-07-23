using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}
