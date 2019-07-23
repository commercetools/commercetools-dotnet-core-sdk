using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class ChangeIsActiveUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "changeIsActive";
        [Required]
        public bool IsActive { get; set; }
    }
}
