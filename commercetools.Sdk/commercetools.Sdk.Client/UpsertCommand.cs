using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class UpsertCommand<T> : Command<T>
    {
        public UpsertCommand(IDraft<T> entity)
        {
            this.Entity = entity;
        }

        public UpsertCommand(IDraft<T> entity, IAdditionalParameters<T> additionalParameters)
        {
            this.Entity = entity;
            this.AdditionalParameters = additionalParameters;
        }

        public IDraft<T> Entity { get; }

        public override System.Type ResourceType => typeof(T);
    }
}
