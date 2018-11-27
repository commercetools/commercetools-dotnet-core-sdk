using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class GetByCustomerIdCommand<T> : GetCommand<T> where T : Cart
    {
        public GetByCustomerIdCommand(string customerId)
        {
            this.ParameterKey = "customerId";
            this.ParameterValue = customerId;
        }
    }
}