namespace commercetools.Sdk.HttpApi
{
    public interface ITokenFlowRegister
    {
        void RegisterFlow(string clientName, TokenFlow tokenFlow);

        TokenFlow GetFlow(string clientName);
    }
}