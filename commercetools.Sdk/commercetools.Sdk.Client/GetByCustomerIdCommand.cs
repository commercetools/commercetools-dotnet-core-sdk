using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class GetByCustomerIdCommand<T> : GetCommand<Cart>
    {
        public override System.Type ResourceType => typeof(T);

        public GetByCustomerIdCommand(string customerId)
        {
            this.ParameterKey = "customerId";
            this.ParameterValue = customerId;
        }
    }
}