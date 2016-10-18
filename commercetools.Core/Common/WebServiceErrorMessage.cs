using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Common
{
    /// <summary>
    /// Represents a single error message returned with a web service error response.
    /// </summary>
    public class WebServiceErrorMessage
    {
        #region Properties

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public WebServiceErrorMessage(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.Code = data.code;
            this.Message = data.message;
        }

        #endregion
    }
}