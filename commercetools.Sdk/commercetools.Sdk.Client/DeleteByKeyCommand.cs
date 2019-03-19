using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class DeleteByKeyCommand<T> : DeleteCommand<T>
    {
        public DeleteByKeyCommand(string key, int version)
        {
            this.Init(key, version);
        }

        public DeleteByKeyCommand(string key, int version, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(key, version);
        }

        public DeleteByKeyCommand(string key, int version, List<Expansion<T>> expandPredicates)
            : base(expandPredicates)
        {
            this.Init(key, version);
        }

        public DeleteByKeyCommand(string key, int version, List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(key, version);
        }

        private void Init(string key, int version)
        {
            this.ParameterKey = Parameters.Key;
            this.ParameterValue = key;
            this.Version = version;
        }
    }
}
