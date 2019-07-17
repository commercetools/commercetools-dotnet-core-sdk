using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts.UpdateActions
{
    public class ChangeSortOrderUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "changeSortOrder";
        [Required]
        public string SortOrder { get; set; }
    }
}