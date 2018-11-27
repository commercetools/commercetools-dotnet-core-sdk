using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class AssetDraft
    {
        public string Key { get; set; }
        public List<AssetSource> Sources { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public List<string> Tags { get; set; }
        public CustomFieldsDraft Custom { get; set; }
    }
}