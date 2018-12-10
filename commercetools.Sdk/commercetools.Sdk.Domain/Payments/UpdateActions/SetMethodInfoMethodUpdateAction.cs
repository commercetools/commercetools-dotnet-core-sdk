using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetMethodInfoMethodUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setMethodInfoMethod";
        public string Method { get; set; }
    }
}