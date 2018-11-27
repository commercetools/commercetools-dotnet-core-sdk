using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Client
{
    public class UpdateByIdCommand<T> : UpdateCommand<T>
    {
        public UpdateByIdCommand(Guid guid, int version, List<UpdateAction<T>> updateActions)
        {
            this.ParameterKey = Parameters.ID;
            this.ParameterValue = guid;
            this.Version = version;
            this.UpdateActions = updateActions;
        }
    }
}