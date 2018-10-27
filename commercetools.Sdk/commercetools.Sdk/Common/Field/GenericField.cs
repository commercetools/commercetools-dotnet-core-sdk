using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public abstract class Field<T> : Dictionary<string, T>, IFields
    {
    }
}