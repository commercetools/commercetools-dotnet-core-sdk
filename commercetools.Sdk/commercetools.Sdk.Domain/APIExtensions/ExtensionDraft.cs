using System.Collections.Generic;

namespace commercetools.Sdk.Domain.APIExtensions
{
    public class ExtensionDraft : IDraft<Extension>
    {
        public string Key { get; set; }

        public Destination Destination { get; set; }

        public List<Trigger> Triggers { get; set; }

        public long? TimeoutInMs { get; set; }
    }
}
