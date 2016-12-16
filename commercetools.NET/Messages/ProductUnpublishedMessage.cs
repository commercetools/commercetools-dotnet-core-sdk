namespace commercetools.Messages
{
    /// <summary>
    /// This message is the result of the unpublish update action.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-messages.html#productunpublished-message"/>
    public class ProductUnpublishedMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ProductUnpublishedMessage(dynamic data)
            : base((object)data)
        {
        }

        #endregion
    }
}
