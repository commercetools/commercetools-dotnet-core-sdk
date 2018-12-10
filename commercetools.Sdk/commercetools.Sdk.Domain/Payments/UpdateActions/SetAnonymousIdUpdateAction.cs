using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetAnonymousIdUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setAnonymousId";
        public string AnonymousId { get; set; }
    }
}