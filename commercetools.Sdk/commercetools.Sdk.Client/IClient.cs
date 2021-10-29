using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// This interface defines the way to communicate with commercetools API.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        /// <value>
        /// The name of the client.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Executes the specified command asynchronously.
        /// </summary>
        /// <typeparam name="T">The domain specific type.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>The object of the domain specific type.</returns>
        Task<T> ExecuteAsync<T>(ICommand<T> command);

        Task<HttpResponseMessage> ExecuteAsyncWithResponse<T>(ICommand<T> command);
    }
}