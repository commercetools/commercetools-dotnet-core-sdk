namespace commercetools.Sdk.Client
{
    using Domain;

    public abstract class Command<T>
    {
        public IAdditionalParameters<T> AdditionalParameters { get; set; }

        public abstract System.Type ResourceType { get; }
    }
}