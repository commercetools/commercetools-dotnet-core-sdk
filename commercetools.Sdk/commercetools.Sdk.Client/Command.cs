using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class Command<T>
    {
        public IAdditionalParameters<T> AdditionalParameters { get; set; }

        public abstract System.Type ResourceType { get; }
    }
}