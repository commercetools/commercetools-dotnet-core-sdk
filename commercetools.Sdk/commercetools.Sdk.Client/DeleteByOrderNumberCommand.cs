using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;

namespace commercetools.Sdk.Client
{
    public class DeleteByOrderNumberCommand : DeleteCommand<Order>
    {
        public DeleteByOrderNumberCommand(string orderNumber, int version)
        {
            this.Init(orderNumber, version);
        }

        public DeleteByOrderNumberCommand(string orderNumber, int version, IAdditionalParameters<Order> additionalParameters)
        {
            this.Init(orderNumber, version);
            this.AdditionalParameters = additionalParameters;
        }

        private void Init(string orderNumber, int version)
        {
            this.ParameterKey = Parameters.OrderNumber;
            this.ParameterValue = orderNumber;
            this.Version = version;
        }
    }
}