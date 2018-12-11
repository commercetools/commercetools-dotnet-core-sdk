using System;

namespace commercetools.Sdk.Client
{
    public class DeleteByIdCommand<T> : DeleteCommand<T>
    {
        public DeleteByIdCommand(Guid id, int version)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
            this.Version = version;
        }
    }
}