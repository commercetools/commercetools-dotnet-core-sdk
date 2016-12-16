using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// A single error message returned by the API.
    /// </summary>
    public class ErrorMessage
    {
        #region Properties

        /// <summary>
        /// Error code
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        #endregion

        #region Construtors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ErrorMessage()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ErrorMessage(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        #endregion
    }
}
