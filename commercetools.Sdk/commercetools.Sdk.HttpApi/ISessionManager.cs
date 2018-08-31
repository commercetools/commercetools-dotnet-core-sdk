namespace commercetools.Sdk.HttpApi
{
    public interface ISessionManager
    {
        string ClientName { get; set; }
        Token Token { get; set; }
        TokenFlow TokenFlow { get; set; }

        // TODO See if these should be split into child interfaces
        string Username { get; set; }

        string Password { get; set; }
        string AnonymousId { get; set; }
    }
}