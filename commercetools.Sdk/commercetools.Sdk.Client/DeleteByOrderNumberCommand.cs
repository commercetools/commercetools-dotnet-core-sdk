using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class DeleteByOrderNumberCommand : DeleteCommand<Order>
    {
        public DeleteByOrderNumberCommand(string orderNumber, int version)
        {
            this.Init(orderNumber, version);
        }

        public DeleteByOrderNumberCommand(string orderNumber, int version, List<Expansion<Order>> expandPredicates)
        : base(expandPredicates)
        {
            this.Init(orderNumber, version);
        }

        public DeleteByOrderNumberCommand(string orderNumber, int version, IAdditionalParameters<Order> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(orderNumber, version);
        }

        public DeleteByOrderNumberCommand(string orderNumber, int version, List<Expansion<Order>> expandPredicates, IAdditionalParameters<Order> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(orderNumber, version);
        }

        private void Init(string orderNumber, int version)
        {
            this.ParameterKey = Parameters.OrderNumber;
            this.ParameterValue = orderNumber;
            this.Version = version;
        }
    }
}
