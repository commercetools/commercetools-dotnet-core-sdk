using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Suggestions
{
    public class SuggestionResult<T>
    {
        public Dictionary<string,List<T>> Suggestions { get; set; }

        public SuggestionResult()
        {
            Suggestions = new Dictionary<string, List<T>>();
        }
    }
}
