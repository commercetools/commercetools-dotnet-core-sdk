namespace commercetools.Sdk.Client
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public class GetByIdCommand<T> : GetCommand<T>
    {
        public GetByIdCommand(Guid id)
        {
            this.Init(id);
        }

        public GetByIdCommand(Guid id, List<Expansion<T>> expand)
            : base(expand)
        {
            this.Init(id);
        }

        private void Init(Guid id)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
        }
    }
}