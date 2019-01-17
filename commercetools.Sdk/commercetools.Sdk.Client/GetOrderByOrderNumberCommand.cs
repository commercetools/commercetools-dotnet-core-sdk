using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class GetOrderByOrderNumberCommand : GetCommand<Order>
    {
        public GetOrderByOrderNumberCommand(string orderNumber)
        {
            this.Init(orderNumber);
        }

        public GetOrderByOrderNumberCommand(string orderNumber, List<Expansion<Order>> expand)
            : base(expand)
        {
            this.Init(orderNumber);
        }

        private void Init(string orderNumber)
        {
            this.ParameterKey = Parameters.OrderNumber;
            this.ParameterValue = orderNumber;
        }
    }
}