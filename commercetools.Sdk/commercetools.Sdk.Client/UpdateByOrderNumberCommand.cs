using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class UpdateByOrderNumberCommand : UpdateCommand<Order>
    {
        public UpdateByOrderNumberCommand(string orderNumber, int version, List<UpdateAction<Order>> updateActions)
        {
           this.Init(orderNumber, version, updateActions);
        }

        public UpdateByOrderNumberCommand(string orderNumber, int version, List<UpdateAction<Order>> updateActions, IAdditionalParameters<Order> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(orderNumber, version, updateActions);
        }

        public UpdateByOrderNumberCommand(string orderNumber, int version, List<UpdateAction<Order>> updateActions, List<Expansion<Order>> expandPredicates)
        : base(expandPredicates)
        {
            this.Init(orderNumber, version, updateActions);
        }

        public UpdateByOrderNumberCommand(string orderNumber, int version, List<UpdateAction<Order>> updateActions, List<Expansion<Order>> expandPredicates, IAdditionalParameters<Order> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(orderNumber, version, updateActions);
        }

        private void Init(string orderNumber, int version, List<UpdateAction<Order>> updateActions)
        {
            this.ParameterKey = Parameters.OrderNumber;
            this.ParameterValue = orderNumber;
            this.Version = version;
            this.UpdateActions.AddRange(updateActions);
        }
    }
}
