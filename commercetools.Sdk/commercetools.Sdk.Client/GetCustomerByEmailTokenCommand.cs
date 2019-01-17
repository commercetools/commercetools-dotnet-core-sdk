using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class GetCustomerByEmailTokenCommand : GetCommand<Customer>
    {
        public GetCustomerByEmailTokenCommand(string emailToken)
        {
            this.Init(emailToken);
        }

        public GetCustomerByEmailTokenCommand(string emailToken, List<Expansion<Customer>> expand)
            : base(expand)
        {
            this.Init(emailToken);
        }

        private void Init(string emailToken)
        {
            this.ParameterKey = Parameters.EmailToken;
            this.ParameterValue = emailToken;
        }
    }
}