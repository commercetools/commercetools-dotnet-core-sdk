using System;
using System.Collections.Generic;

namespace commercetools.Sdk.Registration
{
    public interface IRegisteredTypeRetriever
    {
        IEnumerable<Type> GetRegisteredTypes<T>();
    }
}