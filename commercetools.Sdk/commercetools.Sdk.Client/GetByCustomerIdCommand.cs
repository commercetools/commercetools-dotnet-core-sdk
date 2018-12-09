namespace commercetools.Sdk.Client
{
    using Domain.Carts;

    public class GetByCustomerIdCommand<T> : GetCommand<Cart>
    {
        public GetByCustomerIdCommand(string customerId)
        {
            this.ParameterKey = "customerId";
            this.ParameterValue = customerId;
        }

        public override System.Type ResourceType => typeof(T);
    }
}