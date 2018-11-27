using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Client
{
    public class DeleteByIdCommand<T> : DeleteCommand<T>
    {
        public DeleteByIdCommand(Guid guid, int version)
        {
            this.ParameterKey = Parameters.ID;
            this.ParameterValue = guid;
            this.Version = version;
        }
    }
}
