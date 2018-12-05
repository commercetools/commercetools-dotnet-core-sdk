namespace commercetools.Sdk.Client
{
    using Domain;

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