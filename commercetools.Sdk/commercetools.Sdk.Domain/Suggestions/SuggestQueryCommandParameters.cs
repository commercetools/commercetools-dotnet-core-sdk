namespace commercetools.Sdk.Domain.Suggestions
{
    public class SuggestQueryCommandParameters : IQueryParameters
    {
        public LocalizedString SearchKeywords { get; set; }

        public bool Fuzzy { get; set; }

        public bool Staged { get; set; }

        public int? Limit { get; set; }


        public SuggestQueryCommandParameters()
        {
            Init();
        }

        public SuggestQueryCommandParameters(LocalizedString searchKeywords, bool fuzzy = false,
            bool staged = false, int? limit = null)
        {
            this.SearchKeywords = searchKeywords;
            Init(fuzzy, staged, limit);
        }

        private void Init(bool fuzzy = false,
            bool staged = false, int? limit = null)
        {
            this.Fuzzy = fuzzy;
            this.Staged = staged;
            this.Limit = limit;
        }
    }
}
