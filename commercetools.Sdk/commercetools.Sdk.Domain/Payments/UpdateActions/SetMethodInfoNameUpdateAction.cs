using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetMethodInfoNameUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setMethodInfoName";
        public LocalizedString Name { get; set; }
    }
}