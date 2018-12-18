using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class GetCommand<T> : Command<T>
    {
        protected GetCommand()
        {
            this.Expand = new List<string>();
        }

        protected GetCommand(List<Expansion<T>> expandPredicates)
            : this()
        {
            this.Init(expandPredicates);
        }

        protected GetCommand(List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.Init(expandPredicates);
            this.AdditionalParameters = additionalParameters;
        }

        protected GetCommand(IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.AdditionalParameters = additionalParameters;
        }

        public List<string> Expand { get; }

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