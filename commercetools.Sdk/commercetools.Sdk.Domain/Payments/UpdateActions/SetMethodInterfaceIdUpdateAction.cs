using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetMethodInterfaceIdUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setMethodInfoInterface";
        [Required]
        public string Interface { get; set; }
    }
}