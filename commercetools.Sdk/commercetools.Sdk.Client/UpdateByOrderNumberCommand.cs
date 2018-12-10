using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Client
{
    public class UpdateByOrderNumberCommand : UpdateCommand<Order>
    {
        public UpdateByOrderNumberCommand(string orderNumber, int version, List<UpdateAction<Order>> updateActions)
        {
            this.ParameterKey = Parameters.OrderNumber;
            this.ParameterValue = orderNumber;
            this.Version = version;
            this.UpdateActions.AddRange(updateActions);
        }
    }
}