namespace commercetools.Sdk.Client.Linq
{
    using System;

    [Obsolete("Experimental")]
    public class Api
    {
        public static QueryContext<T> Query<T>()
        {
            return new QueryContext<T>(new QueryCommand<T>());
        }
    }
}
