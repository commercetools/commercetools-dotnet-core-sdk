using System.Collections.Generic;
using System.Linq;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public abstract class GetMatchingQueryCommand<T> : Command<PagedQueryResult<T>>, IExpandable
    {
        protected GetMatchingQueryCommand()
        {
            this.Expand = new List<string>();
            this.UrlSuffix = string.Empty;
        }

        protected GetMatchingQueryCommand(IAdditionalParameters<T> additionalParameters)
        {
            this.AdditionalParameters = additionalParameters;
        }

        public override System.Type ResourceType => typeof(T);

        public List<string> Expand { get; set; }

        public string UrlSuffix { get; set; }

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