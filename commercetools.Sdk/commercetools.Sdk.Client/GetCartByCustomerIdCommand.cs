using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// Get Cart By Customer Id Command
    /// </summary>
    public class GetCartByCustomerIdCommand : GetCommand<Cart>
    {
        public GetCartByCustomerIdCommand(string customerId)
        {
            this.Init(customerId);
        }

        public GetCartByCustomerIdCommand(string customerId, List<Expansion<Cart>> expand)
            : base(expand)
        {
            this.Init(customerId);
        }

        private void Init(string customerId)
        {
            this.ParameterKey = null;
            this.AdditionalParameters = new GetCartByCustomerIdAdditionalParameters()
            {
                CustomerId = customerId
            };
        }
    }
}