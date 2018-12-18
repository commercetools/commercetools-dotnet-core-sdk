using System;
using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public class DeleteByIdCommand<T> : DeleteCommand<T>
    {
        public DeleteByIdCommand(Guid id, int version)
        {
            this.Init(id, version);
        }

        public DeleteByIdCommand(Guid id, int version, IAdditionalParameters<T> additionalParameters)
        {
            this.Init(id, version);
            this.AdditionalParameters = additionalParameters;
        }

        private void Init(Guid id, int version)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
            this.Version = version;
        }
    }
}