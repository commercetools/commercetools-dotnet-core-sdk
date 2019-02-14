using System;

namespace commercetools.Sdk.Client.Linq
{
    [Obsolete("Experimental")]
    public class Api
    {
        public static QueryContext<T> Query<T>()
        {
            return new QueryContext<T>();
        }
    }
}
