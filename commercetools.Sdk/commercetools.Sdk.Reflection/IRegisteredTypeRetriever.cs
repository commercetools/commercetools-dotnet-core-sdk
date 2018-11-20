using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace commercetools.Sdk.Util
{
    public interface IRegisteredTypeRetriever
    {
        IEnumerable<Type> GetRegisteredTypes<T>();
    }
}
