using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class ChangeTaxModeUpdateAction : CartUpdateAction
    {
        public override string Action => "changeTaxMode";
        [Required]
        public TaxMode TaxMode { get; set; }
    }
}