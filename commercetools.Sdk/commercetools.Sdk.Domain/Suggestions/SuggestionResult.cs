using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.Domain.Suggestions
{
    public class SuggestionResult<T> : ISuggestionResult<T> where T : ISuggestion
    {
        public Dictionary<string,List<T>> Suggestions { get; set; }

        public SuggestionResult()
        {
            Suggestions = new Dictionary<string, List<T>>();
        }

        public IEnumerable<string> ToSuggestionList(string locale)
        {
            if (!Suggestions.ContainsKey(locale))
                return null;
            return Suggestions[locale].Select(suggest => suggest.Text);
        }

        public void AddSuggestions(string locale, List<T> suggestions)
        {
            Suggestions?.Add(locale, suggestions);
        }
    }
}
