namespace commercetools.Sdk.Client
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public class UpdateByIdCommand<T> : UpdateCommand<T>
    {
        public UpdateByIdCommand(Guid id, int version, List<UpdateAction<T>> updateActions)
        {
            this.Init(id, version, updateActions);
        }

        public UpdateByIdCommand(Guid id, int version, List<UpdateAction<T>> updateActions, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(id, version, updateActions);
        }

        private void Init(Guid id, int version, List<UpdateAction<T>> updateActions)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
            this.Version = version;
            this.UpdateActions.AddRange(updateActions);
        }
    }
}