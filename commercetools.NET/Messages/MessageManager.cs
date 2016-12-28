using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

using commercetools.Common;

namespace commercetools.Messages
{
    /// <summary>
    /// Provides access to the functions in the Messages section of the API.
    /// </summary>
    public class MessageManager
    {
        #region Constants

        private const string ENDPOINT_PREFIX = "/messages";

        #endregion

        #region Member Variables

        private Client _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public MessageManager(Client client)
        {
            _client = client;
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Gets a Message by its ID.
        /// </summary>
        /// <param name="messageId">Message ID</param>
        /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#get-message-by-id"/>
        /// <returns>Message</returns>
        public Task<Response<Message>> GetMessageByIdAsync(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
            {
                throw new ArgumentException("messageId is required");
            }

            string endpoint = string.Concat(ENDPOINT_PREFIX, "/", messageId);
            return _client.GetAsync<Message>(endpoint);
        }

        /// <summary>
        /// Queries for Messages.
        /// </summary>
        /// <param name="where">Where</param>
        /// <param name="sort">Sort</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns>MessageQueryResult</returns>
        public Task<Response<MessageQueryResult>> QueryMessagesAsync(string where = null, string sort = null, int limit = -1, int offset = -1)
        {
            NameValueCollection values = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(where))
            {
                values.Add("where", where);
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                values.Add("sort", sort);
            }

            if (limit > 0)
            {
                values.Add("limit", limit.ToString());
            }

            if (offset >= 0)
            {
                values.Add("offset", offset.ToString());
            }

            return _client.GetAsync<MessageQueryResult>(ENDPOINT_PREFIX, values);
        }

        #endregion
    }
}
