using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class UpdateByIdCommand<T> : UpdateCommand<T>
    {
        public UpdateByIdCommand(Guid guid, int version, List<UpdateAction> updateActions)
        {
            this.ParameterKey = "id";
            this.ParameterValue = guid;
            this.Version = version;
            this.UpdateActions = updateActions;
        }
    }
}
