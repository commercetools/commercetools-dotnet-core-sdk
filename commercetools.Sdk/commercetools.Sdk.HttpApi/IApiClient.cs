namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Domain;
    using System.Net.Http;

    public interface IApiClient
    {
        HttpClient Client { get; }

        // TODO This is just an example to start with
        // TODO See if we should have only async methods or both
        Category GetCategoryById(int categoryId);
    }
}