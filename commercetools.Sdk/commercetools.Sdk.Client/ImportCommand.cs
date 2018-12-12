using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class ImportCommand<T> : Command<T>
    {
        protected ImportCommand(IImportDraft<T> entity)
        {
            this.Entity = entity;
        }

        public IImportDraft<T> Entity { get; }

        public override System.Type ResourceType => typeof(T);
    }
}