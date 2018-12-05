namespace commercetools.Sdk.Client
{
    using System.Threading.Tasks;

    public interface IClient
    {
        string Name { get; set; }

        Task<T> ExecuteAsync<T>(Command<T> command);
    }
}