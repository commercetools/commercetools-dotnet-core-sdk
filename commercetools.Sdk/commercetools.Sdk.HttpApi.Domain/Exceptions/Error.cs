namespace commercetools.Sdk.HttpApi.Domain
{
    public abstract class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}