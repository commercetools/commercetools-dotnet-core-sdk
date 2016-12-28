using System;

using Newtonsoft.Json;

namespace commercetools.CustomFields
{
    /// <summary>
    /// Custom
    /// </summary>
    /// <remarks>
    /// The CustomFields property maps to the 'custom' field in the API. It could not be named Custom as that is the name of the class.
    /// </remarks>
    /// <see href="http://dev.commercetools.com/http-api-projects-custom-fields.html#custom"/>
    public class Custom
    {
        #region Properties

        /// <summary>
        /// Custom
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public CustomFields CustomFields { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public Custom(dynamic data)
        {
            if (data == null)
            {
                return;
            }

            this.CustomFields = data.custom;
        }

        #endregion
    }
}
