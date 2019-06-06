using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public abstract class DeleteCommand<T> : Command<T>
    {
        protected DeleteCommand()
        {
            this.Expand = new List<string>();
        }

        protected DeleteCommand(List<Expansion<T>> expandPredicates)
        : this()
        {
            this.Init(expandPredicates);
        }

        protected DeleteCommand(IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.AdditionalParameters = additionalParameters;
        }

        protected DeleteCommand(List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.Init(expandPredicates);
            this.AdditionalParameters = additionalParameters;
        }

        public string ParameterKey { get; protected set; }

        public string ParameterValue { get; protected set; }

        public int Version { get; protected set; }

        public bool? DataErasure { get; set; }

        public override System.Type ResourceType => typeof(T);

        public List<string> Expand { get; }

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
