using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetStatusInterfaceCodeUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setStatusInterfaceCode";
        [Required]
        public string InterfaceCode { get; set; }
    }
}