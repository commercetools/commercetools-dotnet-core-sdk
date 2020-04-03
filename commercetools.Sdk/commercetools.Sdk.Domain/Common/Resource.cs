using System;

namespace commercetools.Sdk.Domain.Common
{
    public class Resource<T>: IReferenceable<T>, IVersioned<T>, IIdReferencable<T> where T : Resource<T>
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public ClientLogging CreatedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public ClientLogging LastModifiedBy { get; set; }

        public IReference<T> ToReference()
        {
            return new Reference<T>() { Id = this.Id, Obj = this as T};
        }
    }
}
