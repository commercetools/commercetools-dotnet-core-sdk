using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace commercetools.Common
{
    /// <summary>
    /// Logging handler for the Client methods.
    /// </summary>
    public class ClientLoggingHandler : DelegatingHandler
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="innerHandler"></param>
        public ClientLoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        /// <summary>
        /// Logs the request and response.
        /// </summary>
        /// <param name="request">HttpRequestMessage</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>HttpResponseMessage</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Request: ");
            sb.AppendLine(request.ToString());

            if (request.Content != null)
            {
                sb.AppendLine(await request.Content.ReadAsStringAsync());
            }

            sb.AppendLine();

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            sb.AppendLine("Response: ");
            sb.AppendLine(response.ToString());

            if (response.Content != null)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                sb.AppendLine(responseContent);
            }

            sb.AppendLine();

            Logger.LogInfo(sb.ToString());

            return response;
        }
    }
}
