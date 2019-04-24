using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class DeleteByIdCommand<T> : DeleteCommand<T>
    {
        public DeleteByIdCommand(string id, int version)
        {
            this.Init(id, version);
        }

        public DeleteByIdCommand(string id, int version, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(id, version);
        }

        public DeleteByIdCommand(string id, int version, List<Expansion<T>> expandPredicates)
            : base(expandPredicates)
        {
            this.Init(id, version);
        }

        public DeleteByIdCommand(string id, int version, List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(id, version);
        }

        public DeleteByIdCommand(IVersioned<T> versioned)
        {
            this.Init(versioned.Id, versioned.Version);
        }

        public DeleteByIdCommand(IVersioned<T> versioned, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(versioned.Id, versioned.Version);
        }

        public DeleteByIdCommand(IVersioned<T> versioned, List<Expansion<T>> expandPredicates)
            : base(expandPredicates)
        {
            this.Init(versioned.Id, versioned.Version);
        }

        public DeleteByIdCommand(IVersioned<T> versioned, List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(versioned.Id, versioned.Version);
        }

        public DeleteByIdCommand(Guid id, int version)
        {
            this.Init(id.ToString(), version);
        }

        public DeleteByIdCommand(Guid id, int version, IAdditionalParameters<T> additionalParameters)
        : base(additionalParameters)
        {
            this.Init(id.ToString(), version);
        }

        public DeleteByIdCommand(Guid id, int version, List<Expansion<T>> expandPredicates)
            : base(expandPredicates)
        {
            this.Init(id.ToString(), version);
        }

        public DeleteByIdCommand(Guid id, int version, List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(id.ToString(), version);
        }

        private void Init(string id, int version)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
            this.Version = version;
        }
    }
}
