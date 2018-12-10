using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Client
{
    public class DeleteByOrderNumberCommand : DeleteCommand<Order>
    {
        public DeleteByOrderNumberCommand(string orderNumber, int version)
        {
            this.ParameterKey = Parameters.OrderNumber;
            this.ParameterValue = orderNumber;
            this.Version = version;
        }
    }
}