using System.Collections.Generic;

namespace commercetools.Common
{
    /// <summary>
    /// Container for all API responses.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class Response<T>
    {
        #region Properties

        /// <summary>
        /// Whether or not the request was successful.
        /// </summary>
        /// <remarks>
        /// Only responses with a 200 status code are considered successful.
        /// </remarks>
        public bool Success { get; set; }

        /// <summary>
        /// Result object. Only set when the request was successful.
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Reason Phrase
        /// </summary>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// A list of error messages that were returned. Only set if the request was not successful.
        /// </summary>
        public List<ErrorMessage> Errors { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor;
        /// </summary>
        public Response()
        {
            this.Errors = new List<ErrorMessage>();
        }

        #endregion
    }
}
