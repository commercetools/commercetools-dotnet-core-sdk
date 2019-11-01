using commercetools.Sdk.Domain.Customers;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetCustomerUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setCustomer";
        [Required]
        public IReference<Customer> Customer { get; set; }
    }
}