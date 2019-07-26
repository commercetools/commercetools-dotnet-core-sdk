using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Suggestions;

namespace commercetools.Sdk.Client
{
    public class SuggestQueryCommand<T> : Command<SuggestionResult<T>>
    {
        public SuggestQueryCommand()
        {
           this.QueryParameters = new SuggestQueryCommandParameters();
        }

        public SuggestQueryCommand(SuggestQueryCommandParameters queryParameters)
        {
            this.QueryParameters = queryParameters;
        }

        public override System.Type ResourceType => typeof(T);

        public SuggestQueryCommandParameters QueryParameters { get; set; }

        public void SetSearchKeywords(LocalizedString searchKeywords)
        {
            this.QueryParameters.SearchKeywords = searchKeywords;
        }

        public void SetLimit(int limit)
        {
            this.QueryParameters.Limit = limit;
        }

        public void SetFuzzy(bool fuzzy)
        {
            this.QueryParameters.Fuzzy = fuzzy;
        }

        public void SetStaged(bool staged)
        {
            this.QueryParameters.Staged = staged;
        }
    }
}
