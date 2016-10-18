using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Common
{
    /// <summary>
    /// Thrown when API errors are returned from requests made by the Client.
    /// </summary>
    public class WebServiceHttpException : Exception
    {
        public string StatusCode { get; private set; }
        public string ErrorMessage { get; private set; }
        public List<WebServiceErrorMessage> Errors { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Error response from the API</param>
        public WebServiceHttpException(JObject data)
            : base(GetMessage(data))
        {
            this.StatusCode = data["statusCode"].ToString();
            this.ErrorMessage = data["message"].ToString();
            this.Errors = new List<WebServiceErrorMessage>();

            if (data["errors"] != null)
            {
                foreach (var error in data["errors"])
                {
                    WebServiceErrorMessage message = new WebServiceErrorMessage(error);
                    Errors.Add(message);
                }
            }
        }

        /// <summary>
        /// Gets a message consisting of the HTTP status and the main message in the error response.
        /// </summary>
        /// <param name="data">Error response from the API</param>
        /// <returns>Message</returns>
        private static string GetMessage(JObject data)
        {
            return string.Concat(data["statusCode"], ": ", data["message"]);
        }
    }
}