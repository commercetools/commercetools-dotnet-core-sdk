using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class Command<T>
    {
        public IAdditionalParameters AdditionalParameters { get; protected set; }

        public abstract System.Type ResourceType { get; }
    }
}