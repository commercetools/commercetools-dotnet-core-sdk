namespace commercetools.Sdk.Client
{
    using System;

    public class DeleteByIdCommand<T> : DeleteCommand<T>
    {
        public DeleteByIdCommand(Guid id, int version)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
            this.Version = version;
        }

        public override Type ResourceType => typeof(T);
    }
}