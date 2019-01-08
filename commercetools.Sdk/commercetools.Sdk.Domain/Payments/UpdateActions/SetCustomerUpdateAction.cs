using commercetools.Sdk.Domain.Customers;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Payments.UpdateActions
{
    public class SetCustomerUpdateAction : UpdateAction<Payment>
    {
        public string Action => "setCustomer";
        [Required]
        public Reference<Customer> Customer { get; set; }
    }
}