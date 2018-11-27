using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Util
{
    public interface IRegisteredTypeRetriever
    {
        IEnumerable<Type> GetRegisteredTypes<T>();
    }
}