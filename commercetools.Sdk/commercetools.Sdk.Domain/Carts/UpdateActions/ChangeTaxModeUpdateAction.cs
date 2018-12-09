namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ChangeTaxModeUpdateAction : UpdateAction<Cart>
    {
        public string Action => "changeTaxMode";
        [Required]
        public TaxMode TaxMode { get; set; }
    }
}