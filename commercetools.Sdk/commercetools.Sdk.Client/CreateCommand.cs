using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class CreateCommand<T> : Command<T>
    {
        public CreateCommand(IDraft<T> entity)
        {
            this.Entity = entity;
        }

        public IDraft<T> Entity { get; }

        public override System.Type ResourceType => typeof(T);
    }
}