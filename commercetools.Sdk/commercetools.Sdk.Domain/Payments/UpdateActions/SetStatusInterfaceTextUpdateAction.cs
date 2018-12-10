using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetStatusInterfaceTextUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setStatusInterfaceText";
        [Required]
        public string InterfaceText { get; set; }
    }
}