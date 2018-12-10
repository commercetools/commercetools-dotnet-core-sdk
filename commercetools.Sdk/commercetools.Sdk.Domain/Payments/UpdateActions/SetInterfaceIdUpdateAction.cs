using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetInterfaceIdUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setInterfaceId";
        [Required]
        public string InterfaceId { get; set; }
    }
}