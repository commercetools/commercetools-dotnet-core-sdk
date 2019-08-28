using System.Collections.Generic;

namespace commercetools.Sdk.Domain.APIExtensions
{
    public class Trigger
    {
        public ExtensionResourceType ResourceTypeId { get; set; }

        public List<TriggerType> Actions { get; set; }
    }
}
