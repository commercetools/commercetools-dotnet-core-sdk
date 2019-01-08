using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class ChangeEmailUpdateAction : UpdateAction<Customer>
    {
        public string Action => "changeEmail";
        [Required]
        public string Email { get; set; }
    }
}
