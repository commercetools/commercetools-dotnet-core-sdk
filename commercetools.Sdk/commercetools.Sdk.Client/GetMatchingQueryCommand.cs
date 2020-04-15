﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    [SuppressMessage("ReSharper", "CA2227", Justification = "library api")]
    public abstract class GetMatchingQueryCommand<T> : Command<PagedQueryResult<T>>, IExpandable
    {
        protected GetMatchingQueryCommand()
        {
            this.Expand = new List<string>();
        }

        protected GetMatchingQueryCommand(IAdditionalParameters<T> additionalParameters)
        {
            this.AdditionalParameters = additionalParameters;
        }

        public override System.Type ResourceType => typeof(T);

        public List<string> Expand { get; set; }

        public virtual string UrlSuffix => string.Empty;

        public void SetExpand(IEnumerable<Expansion<T>> expandPredicates)
        {
            if (expandPredicates == null)
            {
                return;
            }

            this.SetExpand(expandPredicates.Select(predicate => predicate.ToString()));
        }

        public void SetExpand(IEnumerable<string> expandPredicates)
        {
            if (expandPredicates == null)
            {
                return;
            }

            this.Expand.Clear();
            foreach (var expand in expandPredicates)
            {
                this.Expand.Add(expand);
            }
        }
    }
}
