using System;
using System.Linq;
using System.Reflection;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain
{
    public static class ResourceIdentifierExtension
    {
        public static ReferenceTypeId GetResourceType<T>(this IReference<T> identifier)
        {
            var attr = typeof(T).GetCustomAttributes(typeof(ResourceTypeAttribute)).FirstOrDefault();
            if (attr != null)
                return ((ResourceTypeAttribute) attr).Value;
            throw new ArgumentException($"Missing ResourceTypeAttribute for type {typeof(T)}" );
        }
    }
}
