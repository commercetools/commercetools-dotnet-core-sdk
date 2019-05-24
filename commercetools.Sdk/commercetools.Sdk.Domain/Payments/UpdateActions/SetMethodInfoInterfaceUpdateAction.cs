using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetMethodInfoInterfaceUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setMethodInfoInterface";
        [Required]
        public string Interface { get; set; }
    }
}
