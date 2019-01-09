using System;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetDateOfBirthUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setDateOfBirth";
        public DateTime DateOfBirth { get; set; }
    }
}
