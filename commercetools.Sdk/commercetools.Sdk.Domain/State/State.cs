using System;

namespace commercetools.Sdk.Domain
{
    [Endpoint("state")]
    [ResourceType(ReferenceTypeId.State)]
    public class State
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Key { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
