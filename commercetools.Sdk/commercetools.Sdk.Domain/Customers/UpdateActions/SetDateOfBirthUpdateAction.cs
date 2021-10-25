using System;

namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    [CustomizeSerializationMarker]
    public class SetDateOfBirthUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setDateOfBirth";
        [AsDateOnly]
        public DateTime DateOfBirth { get; set; }
    }
}
