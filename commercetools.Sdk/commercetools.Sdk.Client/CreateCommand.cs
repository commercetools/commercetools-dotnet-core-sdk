using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class CreateCommand<T> : Command<T>
    {
        public IDraft<T> Entity { get; private set; }
        public override System.Type ResourceType => typeof(T);

        public CreateCommand(IDraft<T> entity)
        {
            this.Entity = entity;
        }
    }
}