using System;

namespace commercetools.Sdk.Domain.CustomObject
{
    public abstract class CustomObjectBase
    {
        public string Id { get; set; }

        public string Container { get; set; }

        public string Key { get; set; }

        public int Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
