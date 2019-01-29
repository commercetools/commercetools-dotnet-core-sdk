using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.ProductDiscounts.UpdateActions
{
    public class ChangeSortOrderUpdateAction : UpdateAction<ProductDiscount>
    {
        public string Action => "changeSortOrder";
        [Required]
        public string SortOrder { get; set; }
    }
}