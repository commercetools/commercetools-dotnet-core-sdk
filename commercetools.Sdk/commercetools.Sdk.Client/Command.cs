namespace commercetools.Sdk.Client
{
    using commercetools.Sdk.Domain;

    public abstract class Command<T>
    {
        public IAdditionalParameters<T> AdditionalParameters { get; set; }
        public abstract System.Type ResourceType { get; } 
    }
}