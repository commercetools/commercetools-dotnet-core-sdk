namespace commercetools.Sdk.Client.Linq
{
    using System;

    [Obsolete("Experimental")]
    public static class Extensions
    {
        public static CtpQueryable<T> Query<T>(this IClient client)
        {
            return new CtpQueryable<T>(client, new QueryCommand<T>());
        }
    }
}
