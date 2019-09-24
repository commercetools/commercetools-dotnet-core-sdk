using System;
namespace commercetools.Sdk.Domain
{
    public interface IResourceTypeIdAsEnum<T> where T : Enum
    {
        T GetResourceTypeIdAsEnum();
    }
}