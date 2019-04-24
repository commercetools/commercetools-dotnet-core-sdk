using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public class UpdateByIdCommand<T> : UpdateCommand<T>
    {
        public UpdateByIdCommand(string id, int version, List<UpdateAction<T>> updateActions)
        {
            this.Init(id, version, updateActions);
        }

        public UpdateByIdCommand(string id, int version, List<UpdateAction<T>> updateActions, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(id, version, updateActions);
        }

        public UpdateByIdCommand(string id, int version, List<UpdateAction<T>> updateActions, List<Expansion<T>> expandPredicates)
            : base(expandPredicates)
        {
            this.Init(id, version, updateActions);
        }

        public UpdateByIdCommand(string id, int version, List<UpdateAction<T>> updateActions, List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(id, version, updateActions);
        }

        public UpdateByIdCommand(IVersioned<T> versioned, List<UpdateAction<T>> updateActions)
        {
            this.Init(versioned.Id, versioned.Version, updateActions);
        }

        public UpdateByIdCommand(IVersioned<T> versioned, List<UpdateAction<T>> updateActions, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(versioned.Id, versioned.Version, updateActions);
        }

        public UpdateByIdCommand(IVersioned<T> versioned, List<UpdateAction<T>> updateActions, List<Expansion<T>> expandPredicates)
            : base(expandPredicates)
        {
            this.Init(versioned.Id, versioned.Version, updateActions);
        }

        public UpdateByIdCommand(IVersioned<T> versioned, List<UpdateAction<T>> updateActions, List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(versioned.Id, versioned.Version, updateActions);
        }

        public UpdateByIdCommand(Guid id, int version, List<UpdateAction<T>> updateActions)
        {
            this.Init(id.ToString(), version, updateActions);
        }

        public UpdateByIdCommand(Guid id, int version, List<UpdateAction<T>> updateActions, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(id.ToString(), version, updateActions);
        }

        public UpdateByIdCommand(Guid id, int version, List<UpdateAction<T>> updateActions, List<Expansion<T>> expandPredicates)
            : base(expandPredicates)
        {
            this.Init(id.ToString(), version, updateActions);
        }

        public UpdateByIdCommand(Guid id, int version, List<UpdateAction<T>> updateActions, List<Expansion<T>> expandPredicates, IAdditionalParameters<T> additionalParameters)
            : base(expandPredicates, additionalParameters)
        {
            this.Init(id.ToString(), version, updateActions);
        }

        private void Init(string id, int version, List<UpdateAction<T>> updateActions)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
            this.Version = version;
            this.UpdateActions.AddRange(updateActions);
        }
    }
}
