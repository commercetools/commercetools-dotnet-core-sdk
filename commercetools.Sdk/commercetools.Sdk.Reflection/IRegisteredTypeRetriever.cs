using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace commercetools.Sdk.Extensions
{
    public interface IRegisteredTypeRetriever
    {
        IEnumerable<Type> GetRegisteredTypes<T>();
    }
}
