namespace commercetools.Sdk.Client.Linq
{
    public class Api
    {
        public static QueryContext<T> Query<T>(IClient client)
        {
            return new QueryContext<T>(client);
        }
    }
}
