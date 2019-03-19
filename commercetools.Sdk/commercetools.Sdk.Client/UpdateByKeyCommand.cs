using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class UpdateByKeyCommand<T> : UpdateCommand<T>
    {
        public UpdateByKeyCommand(string key, int version, List<UpdateAction<T>> updateActions)
        {
            this.Init(key, version, updateActions);
        }

        public UpdateByKeyCommand(string key, int version, List<UpdateAction<T>> updateActions, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(key, version, updateActions);
        }

        public UpdateByKeyCommand(string key, int version, List<UpdateAction<T>> updateActions, List<Expansion<T>> expandPredicates)
            : base(expandPredicates)
        {
            this.Init(key, version, updateActions);
        }

        public UpdateByKeyCommand(string key, int version, List<UpdateAction<T>> updateActions, List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(key, version, updateActions);
        }

        private void Init(string key, int version, List<UpdateAction<T>> updateActions)
        {
            this.ParameterKey = Parameters.Key;
            this.ParameterValue = key;
            this.Version = version;
            this.UpdateActions.AddRange(updateActions);
        }
    }
}
