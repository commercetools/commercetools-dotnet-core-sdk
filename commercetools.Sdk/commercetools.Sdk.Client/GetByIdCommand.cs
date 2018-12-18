using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
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

        public GetByIdCommand(Guid id, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(id);
        }

        public GetByIdCommand(Guid id, List<Expansion<T>> expand, IAdditionalParameters<T> additionalParameters)
            : base(expand, additionalParameters)
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