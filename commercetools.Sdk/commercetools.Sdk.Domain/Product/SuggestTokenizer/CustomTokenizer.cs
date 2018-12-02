using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    [TypeMarker("custom")]
    public class CustomTokenizer : SuggestTokenizer
    {
        public List<string> Inputs { get; set; }
    }
}