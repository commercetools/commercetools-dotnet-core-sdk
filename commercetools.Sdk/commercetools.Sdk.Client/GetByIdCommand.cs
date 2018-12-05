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

        public override System.Type ResourceType => typeof(T);

        private void Init(Guid id)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
        }
    }
}