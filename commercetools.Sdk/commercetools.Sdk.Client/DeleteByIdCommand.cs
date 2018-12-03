using System;

namespace commercetools.Sdk.Client
{
    public class DeleteByIdCommand<T> : DeleteCommand<T>
    {
        public override System.Type ResourceType => typeof(T);

        public DeleteByIdCommand(Guid guid, int version)
        {
            this.ParameterKey = Parameters.ID;
            this.ParameterValue = guid;
            this.Version = version;
        }
    }
}