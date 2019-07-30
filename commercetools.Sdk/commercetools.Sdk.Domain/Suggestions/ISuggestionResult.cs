using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Suggestions
{
    public interface ISuggestionResult<T>
    {
        Dictionary<string,List<T>> Suggestions { get; set; }

        IEnumerable<string> ToSuggestionList(string locale);

        void AddSuggestions(string locale, List<T> suggestions);
    }
}
