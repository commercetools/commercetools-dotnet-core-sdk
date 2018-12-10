namespace commercetools.Sdk.Client
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public class UpdateByIdCommand<T> : UpdateCommand<T>
    {
        public UpdateByIdCommand(Guid id, int version, List<UpdateAction<T>> updateActions)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
            this.Version = version;
            this.UpdateActions.AddRange(updateActions);
        }
    }
}