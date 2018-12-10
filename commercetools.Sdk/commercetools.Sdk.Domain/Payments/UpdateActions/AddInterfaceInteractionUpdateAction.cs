using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class AddInterfaceInteractionUpdateAction : UpdateAction<Payment>
    {
        public string Action => "addInterfaceInteraction";
        [Required]
        public ResourceIdentifier Type { get; set; }
        public CustomFields Fields { get; set; }
    }
}