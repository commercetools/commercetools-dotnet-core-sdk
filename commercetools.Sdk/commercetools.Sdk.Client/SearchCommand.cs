using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class SearchCommand<T> : Command<PagedQueryResult<T>>
    {
        protected SearchCommand(ISearchParameters<T> searchParameters)
        {
            this.SearchParameters = searchParameters;
        }

        protected SearchCommand(ISearchParameters<T> searchParameters, IAdditionalParameters<T> additionalParameters)
        {
            this.SearchParameters = searchParameters;
            this.AdditionalParameters = additionalParameters;
        }

        public ISearchParameters<T> SearchParameters { get; }

        public override System.Type ResourceType => typeof(T);
    }
}