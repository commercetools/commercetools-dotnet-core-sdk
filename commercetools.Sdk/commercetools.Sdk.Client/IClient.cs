using System.Threading.Tasks;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(Command<T> command);
    }
}