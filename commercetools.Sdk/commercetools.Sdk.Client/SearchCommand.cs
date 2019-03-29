using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public abstract class SearchCommand<T> : Command<PagedQueryResult<T>>
    {
        protected SearchCommand()
        {
            this.Expand = new List<string>();
        }

        protected SearchCommand(ISearchParameters<T> searchParameters)
            : this()
        {
            this.SearchParameters = searchParameters;
        }

        protected SearchCommand(ISearchParameters<T> searchParameters, IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.SearchParameters = searchParameters;
            this.AdditionalParameters = additionalParameters;
        }

        protected SearchCommand(List<Expansion<T>> expandPredicates, ISearchParameters<T> searchParameters, IAdditionalParameters<T> additionalParameters)
            : this()
        {
            this.Init(expandPredicates);
            this.SearchParameters = searchParameters;
            this.AdditionalParameters = additionalParameters;
        }

        public List<string> Expand { get; }

        public ISearchParameters<T> SearchParameters { get; }

        public override System.Type ResourceType => typeof(T);

        private void Init(List<Expansion<T>> expandPredicates)
        {
            if (expandPredicates == null)
            {
                return;
            }

            foreach (var expand in expandPredicates)
            {
                this.Expand.Add(expand.ToString());
            }
        }
    }
}
