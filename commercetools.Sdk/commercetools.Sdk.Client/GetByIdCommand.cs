namespace commercetools.Sdk.Client
{
    using commercetools.Sdk.Domain;
    using System;
    using System.Collections.Generic;

    public class GetByIdCommand<T> : GetCommand<T>
    {
        public GetByIdCommand(Guid guid)
        {
            Init(guid);
        }

        public GetByIdCommand(Guid guid, List<Expansion<T>> expand) : base(expand)
        {
            Init(guid);
        }

        private void Init(Guid guid)
        {
            this.ParameterKey = Parameters.ID;
            this.ParameterValue = guid;
        }
    }
}