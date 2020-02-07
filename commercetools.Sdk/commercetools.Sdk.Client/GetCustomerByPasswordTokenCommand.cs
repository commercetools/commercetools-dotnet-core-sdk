using System.Collections.Generic;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class GetCustomerByPasswordTokenCommand : GetCommand<Customer>
    {
        public GetCustomerByPasswordTokenCommand(string passwordToken)
        {
            this.Init(passwordToken);
        }

        public GetCustomerByPasswordTokenCommand(string passwordToken, List<Expansion<Customer>> expand)
            : base(expand)
        {
            this.Init(passwordToken);
        }

        private void Init(string passwordToken)
        {
            this.ParameterKey = Parameters.PasswordToken;
            this.ParameterValue = passwordToken;
        }
    }
}