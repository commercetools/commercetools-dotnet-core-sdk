using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class CreateCommand<T> : Command<T>
    {
        public IDraft<T> Entity { get; private set; }

        public CreateCommand(IDraft<T> entity)
        {
            this.Entity = entity;
        }
    }
}