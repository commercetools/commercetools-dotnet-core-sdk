using System;
using commercetools.Sdk.Domain.Customers;

namespace commercetools.Sdk.Domain.Messages.Customers
{
    [TypeMarker("CustomerDateOfBirthSet")]
    public class CustomerDateOfBirthSetMessage : Message<Customer>
    {
        public DateTime DateOfBirth { get; set;}
    }
}
