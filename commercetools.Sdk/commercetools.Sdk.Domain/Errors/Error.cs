namespace commercetools.Sdk.Domain.Errors
{
    public abstract class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}