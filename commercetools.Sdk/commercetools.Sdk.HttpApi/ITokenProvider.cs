namespace commercetools.Sdk.HttpApi
{
    public interface ITokenProvider
    {
        Token Token { get; }
        TokenFlow TokenFlow { get; }
    }
}