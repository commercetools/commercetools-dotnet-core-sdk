using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    using System.Collections.Generic;
    using Domain;

    public abstract class UpdateCommand<T> : Command<T>
    {
        protected UpdateCommand()
        {
            this.UpdateActions = new List<UpdateAction<T>>();
            this.Expand = new List<string>();
        }

        protected UpdateCommand(List<Expansion<T>> expandPredicates)
            : this()
        {
            this.Init(expandPredicates);
        }

        protected UpdateCommand(IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.AdditionalParameters = additionalParameters;
        }

        protected UpdateCommand(List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.Init(expandPredicates);
            this.AdditionalParameters = additionalParameters;
        }

        public List<string> Expand { get; }

        public int Version { get; protected set; }

        public List<UpdateAction<T>> UpdateActions { get; }

        public string ParameterKey { get; protected set; }

        public object ParameterValue { get; protected set; }

        public override System.Type ResourceType => typeof(T);

        private void Init(List<Expansion<T>> expandPredicates)
        {
            if (expandPredicates == null)
            {
                return;
            }

            foreach (var expand in expandPredicates)
            {
                this.Expand.Add(expand.ToString());
            }
        }
    }
}
